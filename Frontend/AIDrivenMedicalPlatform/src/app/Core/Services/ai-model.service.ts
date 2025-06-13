import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AiModelService {
  constructor(private http: HttpClient) {}

  sendImage(formData: FormData, model:string): Observable<any> {
    return this.http.post(`http://localhost:8000/${model}`, formData);
  }

  saveMedicalImageAiAnalysis(AiAnalysisData : any){
    return this.http.post(`${environment.apiBaseURL}/Appointment/SetMedicalImageAiAnalysis`, AiAnalysisData);
  }

  getMedicalImageAiAnalysis(medicalImageId : any){
    return this.http.get(`${environment.apiBaseURL}/Appointment/GetMedicalImageAiAnalysis/${medicalImageId}`);
  }
}