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
    return this._http.post(`${environment.apiBaseURL}/MedicalImage/AddMedicalImage`, medicalImage)
  }

  RemoveMedicalImage(medicalImageId: number) {
    let params = new HttpParams();
    if (medicalImageId) params = params.set('medicalImageId', medicalImageId);

    return this._http.delete(`${environment.apiBaseURL}/MedicalImage/DeleteMedicalImage`, { params })
  }

  GetMedicalImage(appointmentId: number) {
    let params = new HttpParams();
    if (appointmentId) params = params.set('appointmentId', appointmentId);
    return this._http.get(`${environment.apiBaseURL}/MedicalImage/GetMedicalImage`, { params })
  }

  EditMedicalImage(newImage: string, medicalImageId: number) {
    const body = { Image:newImage };
    let params = new HttpParams();
    if (medicalImageId) params = params.set('medicalImageId', medicalImageId);
    return this._http.patch(`${environment.apiBaseURL}/MedicalImage/EditMedicalImage`, body, { params });
  }
}
