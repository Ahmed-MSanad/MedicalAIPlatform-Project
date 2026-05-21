import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IllnessChoices } from '../Enums/illness-choices';

@Injectable({
  providedIn: 'root'
})
export class AiModelService {
  constructor(private http: HttpClient) {}

  sendImageAtelectasisModel(formData: FormData): Observable<any> {
    return this.http.post(`http://localhost:8000/${IllnessChoices.Atelectasis}`, formData);
  }
  sendImageEffusionModel(formData: FormData): Observable<any> {
    return this.http.post(`http://localhost:8000/${IllnessChoices.Effusion}`, formData);
  }
  sendImageInfiltrationModel(formData: FormData): Observable<any> {
    return this.http.post(`http://localhost:8000/${IllnessChoices.Infiltration}`, formData);
  }

  saveMedicalImageAiAnalysis(aiAnalysisData: any) {
    return this.http.post(
      `${environment.apiBaseURL}/api/medicalaidata/medical-images/analysis`,
      aiAnalysisData
    );
  }

  getMedicalImageAiAnalysis(medicalImageId: number) {
    return this.http.get(
      `${environment.apiBaseURL}/api/medicalaidata/medical-images/${medicalImageId}/analysis`
    );
  }

  getMedicalImageOwner(medicalImageId: number) {
    return this.http.get(
      `${environment.apiBaseURL}/api/medicalaidata/medical-images/${medicalImageId}/patient`
    );
  }

  getAiAnalysisDoctorData(doctorId: string) {
    return this.http.get(
      `${environment.apiBaseURL}/api/medicalaidata/doctors/${doctorId}/ai-data`
    );
  }
}