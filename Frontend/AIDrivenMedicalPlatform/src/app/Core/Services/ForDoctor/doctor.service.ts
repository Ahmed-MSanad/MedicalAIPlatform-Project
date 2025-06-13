import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  constructor(private _http:HttpClient) { }

  getDoctorSchedule(){
    return this._http.get(environment.apiBaseURL + '/Schedule/GetDoctorSchedule')
  }
  updateDoctorSchedule(updatedSchedule: any){
    return this._http.put(environment.apiBaseURL + '/Schedule/EditDoctorSchedule', updatedSchedule)
  }

  getDoctorProfile(){
    return this._http.get(environment.apiBaseURL+'/Profile/GetDoctorProfile');
  }
  updateDoctorProfile(updatedUser: any) {
    return this._http.put(environment.apiBaseURL + '/Profile/EditDoctorProfile', updatedUser);
  }
}
