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

  saveMedicalImageAiAnalysis(AiAnalysisData : any){
    return this.http.post(`${environment.apiBaseURL}/MedicalAIData/SetMedicalImageAiAnalysis`, AiAnalysisData);
  }

  getMedicalImageAiAnalysis(medicalImageId : any){
    return this.http.get(`${environment.apiBaseURL}/MedicalAIData/GetMedicalImageAiAnalysis/${medicalImageId}`);
  }

  getMedicalImageOwner(medicalImageId : any){
    return this.http.get(`${environment.apiBaseURL}/MedicalAIData/GetMedicalImageOwner/${medicalImageId}`);
  }
}