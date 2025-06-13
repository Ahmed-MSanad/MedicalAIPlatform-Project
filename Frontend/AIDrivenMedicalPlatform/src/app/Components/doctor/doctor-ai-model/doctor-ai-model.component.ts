import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Component, inject, signal, WritableSignal } from '@angular/core';
import { AiModelService } from '../../../Core/Services/ai-model.service';
import { CommonModule } from '@angular/common';
import { IllnessChoices } from '../../../Core/Enums/illness-choices';

@Component({
  selector: 'app-doctor-ai-model',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './doctor-ai-model.component.html',
  styleUrl: './doctor-ai-model.component.scss'
})
export class DoctorAiModelComponent {
  private readonly formBuilder = inject(FormBuilder);

  aiModelForm = this.formBuilder.group({
    ConfidenceScore: [0 as number, [Validators.required]],
    ExplanationDetails: ["", [Validators.required]],
    Diagnosis: ["", [Validators.required]],
    image: [null as string | ArrayBuffer | null],
    MedicalImageId: [0 as number, [Validators.required]]
  });

  selectedFile: File | null = null;

  private readonly aiModelService = inject(AiModelService);

  displayImage: WritableSignal<string> = signal("");
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      console.log('Selected file:', this.selectedFile);
      this.modelImage.set("");
      this.classification.set("");
      this.predicted_probability.set("");
      const reader = new FileReader();
      reader.onload = (e) => {
        if (e.target?.result) {
          this.displayImage.set(e.target.result as string);
        }
      };
      reader.readAsDataURL(this.selectedFile);
    } else {
      console.log('No files selected');
      this.selectedFile = null;
      this.displayImage.set("");
    }
  }

  classification : WritableSignal<string> = signal("");
  predicted_probability : WritableSignal<string> = signal("");
  modelImage: WritableSignal<string> = signal(""); // signal for saliency map
  illnessChoice : WritableSignal<string> = signal(IllnessChoices.Atelectasis);
  illnessOptions = Object.entries(IllnessChoices);
  submitImage(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name);

      for (const [key, value] of formData.entries()) {
        console.log(`${key}:`, value);
      }

      this.aiModelService.sendImage(formData, this.illnessChoice()).subscribe({
        next: (res) => {
          console.log('Server response:', res);
          this.classification.set(res.prediction);
          this.predicted_probability.set(res.predicted_probability);

          this.aiModelForm.get("Diagnosis")?.setValue(res.prediction);
          this.aiModelForm.get("ConfidenceScore")?.setValue(parseFloat(res.predicted_probability.replace('%', '')));

          // Update modelImage if saliency_map is present
          if (res.saliency_map) {

            this.modelImage.set(res.saliency_map);
            const base64 = (res.saliency_map as string).split(',')[1];
            this.aiModelForm.get('image')?.setValue(base64);

          } else {
            this.modelImage.set(""); // Clear if no saliency map
            this.aiModelForm.get('image')?.setValue("");
          }
        },
        error: (err) => {
          console.error('Server error:', err);
        }
      });
    } else {
      console.log('No file selected:', this.selectedFile);
    }
  }

  saveAiAnalysis(){
    console.log(this.aiModelForm.value);
    console.log('Form value:', JSON.stringify(this.aiModelForm.value, null, 2));
    if (this.aiModelForm.valid) {
      this.aiModelService.saveMedicalImageAiAnalysis(this.aiModelForm.value).subscribe({
        next: () => {
          console.log('AI analysis saved successfully');
        },
        error: (err) => {
          console.error('Error saving AI analysis:', err.error); // Log the backend error
        }
      });
    } else {
      console.log('Form is invalid:', this.aiModelForm.errors);
    }
  }
}