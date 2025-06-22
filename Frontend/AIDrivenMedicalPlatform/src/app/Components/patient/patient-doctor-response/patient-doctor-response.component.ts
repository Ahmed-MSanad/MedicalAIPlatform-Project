import { CommonModule } from '@angular/common';
import { AiModelService } from './../../../Core/Services/ai-model.service';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

export interface AiAnalysisResponse {
  confidenceScore: number;   
  explanationDetails: string;
  diagnosis: string;         
  image: string;             
  medicalImageId: number;    
}

@Component({
  selector: 'app-patient-doctor-response',
  imports: [FormsModule, CommonModule, TranslateModule],
  templateUrl: './patient-doctor-response.component.html',
  styleUrl: './patient-doctor-response.component.scss'
})
export class PatientDoctorResponseComponent implements OnInit{
  medicalImageId : number = 0;
  private readonly aiModelService = inject(AiModelService);
  responseData ! : AiAnalysisResponse[];
  private readonly activatedRoute = inject(ActivatedRoute);
  isLoading:boolean = false;

  ngOnInit(): void {
      this.activatedRoute.paramMap.subscribe((paramList) => {
        this.medicalImageId = parseInt(paramList.get("medicalImageId") ?? "0");
        console.log(`medical Image Id = ${this.medicalImageId}`);
        this.getMedicalAiAnalysis();
      });
  }

  getMedicalAiAnalysis(){
    this.isLoading = true;
    this.aiModelService.getMedicalImageAiAnalysis(this.medicalImageId).subscribe({
      next:(res : any) => {
        this.isLoading = false;
        this.responseData = res;
        console.log(res);
      },
      error:(err) => {
        this.isLoading = false;
        console.log(err.error);
      }
    });
  }

  getImageDataUrl(base64String: string): string {
    return `data:image/png;base64,${base64String}`;
  }
}
