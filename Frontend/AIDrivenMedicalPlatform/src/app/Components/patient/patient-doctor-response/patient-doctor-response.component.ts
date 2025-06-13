import { CommonModule } from '@angular/common';
import { AiModelService } from './../../../Core/Services/ai-model.service';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

export interface AiAnalysisResponse {
  confidenceScore: number;   
  explanationDetails: string;
  diagnosis: string;         
  image: string;             
  medicalImageId: number;    
}

@Component({
  selector: 'app-patient-doctor-response',
  imports: [FormsModule, CommonModule],
  templateUrl: './patient-doctor-response.component.html',
  styleUrl: './patient-doctor-response.component.scss'
})
export class PatientDoctorResponseComponent {
  medicalImageId : number = 0;
  private readonly aiModelService = inject(AiModelService);
  responseData ! : AiAnalysisResponse;
  getMedicalAiAnalysis(){
    this.aiModelService.getMedicalImageAiAnalysis(this.medicalImageId).subscribe({
      next:(res : any) => {
        this.responseData = res;
        console.log(res);
      },
      error:(err) => {
        console.log(err.error);
      }
    });
  }

  getImageDataUrl(base64String: string): string {
    return `data:image/png;base64,${base64String}`;
  }
}
