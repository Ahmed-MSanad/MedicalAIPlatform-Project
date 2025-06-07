import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class PatientService {

  constructor(private _http : HttpClient) { }

  getPatientProfile(){
    return this._http.get(environment.apiBaseURL+'/Profile/GetPatientProfile');
  }
  updatePatientProfile(updatedUser: any) {
    return this._http.put(environment.apiBaseURL + '/Profile/EditPatientProfile', updatedUser);
  }
  
}
