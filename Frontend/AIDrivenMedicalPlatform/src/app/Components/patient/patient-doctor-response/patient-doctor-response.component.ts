import { CommonModule } from '@angular/common';
import { AiModelService } from './../../../Core/Services/ai-model.service';
import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { DoctorInfo } from '../../../Core/Interfaces/doctor-info';

export interface AiAnalysisResponse {
  confidenceScore: number;   
  explanationDetails: string;
  diagnosis: string;         
  image: string;             
  medicalImageId: number;
  diseaseType: string;
}

@Component({
  selector: 'app-patient-doctor-response',
  imports: [FormsModule, CommonModule, TranslateModule],
  templateUrl: './patient-doctor-response.component.html',
  styleUrl: './patient-doctor-response.component.scss'
})
export class PatientDoctorResponseComponent implements OnInit{
  medicalImageId : number = 0;
  doctorId : string = "";
  private readonly aiModelService = inject(AiModelService);
  responseData ! : AiAnalysisResponse[];
  private readonly activatedRoute = inject(ActivatedRoute);
  isLoading:boolean = false;
  doctorData : WritableSignal<DoctorInfo> = signal({} as DoctorInfo);

  ngOnInit(): void {
      this.activatedRoute.paramMap.subscribe((paramList) => {
        this.medicalImageId = parseInt(paramList.get("medicalImageId") ?? "0");
        this.doctorId = paramList.get("doctorId") ?? "";
        console.log(`medical Image Id = ${this.medicalImageId}`);
        this.getMedicalAiAnalysis();
        this.getDoctorData();
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

  getDoctorData(){
    this.aiModelService.getAiAnalysisDoctorData(this.doctorId).subscribe({
      next:(res : any) => {
        this.doctorData.set(res);
        console.log("doctorData", this.doctorData);
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
