import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MedicalImage } from '../Interfaces/medical-image';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MedicalImageService {

  constructor(private _http: HttpClient) { }

  CreateMedicalImage(medicalImage: MedicalImage) {
    return this._http.post(
      `${environment.apiBaseURL}/api/medical-images`,
      medicalImage
    );
  }

  RemoveMedicalImage(medicalImageId: number) {
    return this._http.delete(
      `${environment.apiBaseURL}/api/medical-images/${medicalImageId}`
    );
  }

  GetMedicalImage(appointmentId: number) {
    return this._http.get(
      `${environment.apiBaseURL}/api/appointments/${appointmentId}/medical-images`
    );
  }

  EditMedicalImage(newImage: string, medicalImageId: number) {
    const body = { image: newImage };
    return this._http.patch(
      `${environment.apiBaseURL}/api/medical-images/${medicalImageId}`,
      body
    );
  }
}
