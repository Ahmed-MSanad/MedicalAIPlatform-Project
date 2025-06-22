import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { AiModelService } from '../../../Core/Services/ai-model.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserProfile } from '../../../Core/Interfaces/user-profile';
import { Gender } from '../../../Core/Enums/gender';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-doctor-ai-model',
  imports: [ReactiveFormsModule, CommonModule, TranslateModule],
  templateUrl: './doctor-ai-model.component.html',
  styleUrl: './doctor-ai-model.component.scss'
})
export class DoctorAiModelComponent implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly aiModelService = inject(AiModelService);
  private readonly activatedRoute = inject(ActivatedRoute);

  originalImage: WritableSignal<string> = signal("");
  MedicalImageId: number = 0;
  patientDetails: UserProfile = {} as UserProfile;
  isLoading: boolean = false;
  ngOnInit() {
    this.activatedRoute.paramMap.subscribe({
      next: (paramList) => {
        this.originalImage.set(paramList.get("image") ?? "");
        this.MedicalImageId = parseInt(paramList.get('id') ?? "0");
        if (this.MedicalImageId !== 0) {
          this.isLoading = true;
          this.aiModelService.getMedicalImageOwner(this.MedicalImageId).subscribe({
            next: (res: any) => {
              this.isLoading = false;
              this.patientDetails = {
                email: res.email,
                fullName: res.fullName,
                dateOfBirth: res.dateOfBirth ? new Date(res.dateOfBirth) : new Date(0, 0, 0),
                gender: res.gender === 0 ? Gender.Male : Gender.Female,
                address: res.address,
                occupation: res.occupation,
                emergencyContactName: res.emergencyContactName,
                emergencyContactNumber: res.emergencyContactNumber,
                familyMedicalHistory: res.familyMedicalHistory,
                pastMedicalHistory: res.pastMedicalHistory,
                phone: res.patientPhones && res.patientPhones.length > 0 ? res.patientPhones[0] : '',
                imagePath: res.image ? `data:image/jpeg;base64,${res.image}` : ''
              };

            }
          });

          this.submitImage();
        }
      }
    });
  }

  aiModelForm = this.formBuilder.group({
    ConfidenceScore: [0 as number],
    Diagnosis: [""],
    image: [null as string | ArrayBuffer | null],
    ExplanationDetails: ["", [Validators.required]],
    MedicalImageId: [0 as number, [Validators.required]]
  });

  selectedFile: File | null = null;

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      console.log('Selected file:', this.selectedFile);
      this.modelImage.set([]);
      this.classification.set([]);
      this.predicted_probability.set([]);
      const reader = new FileReader();
      reader.onload = (e) => {
        if (e.target?.result) {
          this.originalImage.set(e.target.result as string);
        }
      };
      reader.readAsDataURL(this.selectedFile);
    } else {
      console.log('No files selected');
      this.selectedFile = null;
      this.originalImage.set("");
    }
  }


  submitImage(): void {
    if (this.MedicalImageId != 0 && this.originalImage() != "") {
      const blob = this.base64ToBlob(this.originalImage(), 'image/png');
      const file = new File([blob], 'image.png', { type: 'image/png' });
      if (file) {
        const formData = new FormData();
        formData.append('file', file);
        for (const [key, value] of formData.entries()) {
          console.log(`${key}:`, value);
        }

        this.aiModelForm.get('MedicalImageId')?.setValue(this.MedicalImageId);

        this.sendToAllModels(formData);
      }
    }
    else if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name);

      for (const [key, value] of formData.entries()) {
        console.log(`${key}:`, value);
      }

      this.sendToAllModels(formData);
    }
    else {
      console.log('No file selected:', this.selectedFile);
    }
  }

  classification: WritableSignal<string[]> = signal([]);
  predicted_probability: WritableSignal<string[]> = signal([]);
  modelImage: WritableSignal<string[]> = signal([]);
  forForLoop = Array.from({ length: 3 }, (_, i) => i);
  //------------------------------------------------------------
  private sendToAllModels(formData: FormData): void {
    this.isLoading = true;
    this.aiModelService.sendImageAtelectasisModel(formData).subscribe({
      next: (res) => {
        this.isLoading = false;
        // console.log('Server response:', res);
        this.classification.set([...this.classification(), res.prediction]);
        this.predicted_probability.set([...this.predicted_probability(), res.predicted_probability]);

        if (res.saliency_map) {
          this.modelImage.set([...this.modelImage(), res.saliency_map]);
        } else {
          this.modelImage.set([...this.modelImage(), ""]);
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Server error:', err);
      }
    });
    this.isLoading = true;
    this.aiModelService.sendImageEffusionModel(formData).subscribe({
      next: (res) => {
        this.isLoading = false;
        // console.log('Server response:', res);
        this.classification.set([...this.classification(), res.prediction]);
        this.predicted_probability.set([...this.predicted_probability(), res.predicted_probability]);

        if (res.saliency_map) {
          this.modelImage.set([...this.modelImage(), res.saliency_map]);
        } else {
          this.modelImage.set([...this.modelImage(), ""]);
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Server error:', err);
      }
    });
    this.isLoading = true;
    this.aiModelService.sendImageInfiltrationModel(formData).subscribe({
      next: (res) => {
        this.isLoading = false;
        // console.log('Server response:', res);
        this.classification.set([...this.classification(), res.prediction]);
        this.predicted_probability.set([...this.predicted_probability(), res.predicted_probability]);

        if (res.saliency_map) {
          this.modelImage.set([...this.modelImage(), res.saliency_map]);
        } else {
          this.modelImage.set([...this.modelImage(), ""]);
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Server error:', err);
      }
    });
  }

  base64ToBlob(base64: string, contentType = 'image/png'): Blob {
    const byteCharacters = atob(base64.split(',')[1]);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      const slice = byteCharacters.slice(offset, offset + 512);
      const byteNumbers = new Array(slice.length);

      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
    return new Blob(byteArrays, { type: contentType });
  }

  private readonly toastr = inject(ToastrService);
  saveAiAnalysis() {
    console.log(this.aiModelForm.value);
    console.log('Form value:', JSON.stringify(this.aiModelForm.value, null, 2));

    for (let i = 0; i < 3; i++) {
      this.aiModelForm.get("Diagnosis")?.setValue(this.classification()[i]);
      this.aiModelForm.get("ConfidenceScore")?.setValue(parseFloat(this.predicted_probability()[i].replace('%', '')));
      const base64 = (this.modelImage()[i] as string).split(',')[1];
      this.aiModelForm.get('image')?.setValue(base64);

      if (this.aiModelForm.valid) {
        this.isLoading = true;
        this.aiModelService.saveMedicalImageAiAnalysis(this.aiModelForm.value).subscribe({
          next: (res: any) => {
            this.isLoading = false;
            console.log(res.message);
            if (i == 2)
              this.toastr.success(res.message, "Wonderful🎆🥳")
          },
          error: (err) => {
            this.isLoading = false;
            this.toastr.warning(err.error.error, "Warning");
            console.log(err);
          }
        });
      } else {
        console.log('Form is invalid:', this.aiModelForm.errors);
      }
    }

  }

}