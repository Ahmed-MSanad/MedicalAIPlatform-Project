import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _http : HttpClient){}

  getProfile() {
    return this._http.get(`${environment.apiBaseURL}/api/profile`);
  }

  deleteUserProfile() {
    return this._http.delete(`${environment.apiBaseURL}/api/profile`);
  }

  updateAdminProfile(updatedUser: any) {
    return this._http.put(`${environment.apiBaseURL}/api/profile/admin`, updatedUser);
  }
  updateDoctorProfile(updatedUser: any) {
    return this._http.put(`${environment.apiBaseURL}/api/profile/doctor`, updatedUser);
  }
  updatePatientProfile(updatedUser: any) {
    return this._http.put(`${environment.apiBaseURL}/api/profile/patient`, updatedUser);
  }
}
