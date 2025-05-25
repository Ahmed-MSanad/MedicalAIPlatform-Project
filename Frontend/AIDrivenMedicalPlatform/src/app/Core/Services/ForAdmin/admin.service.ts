import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private _http:HttpClient) { }

  getAdminProfile(){
      return this._http.get(environment.apiBaseURL+'/profile/GetAdminProfile');
    }
    updateAdminProfile(updatedUser: any) {
      return this._http.put(environment.apiBaseURL + '/profile/EditAdminProfile', updatedUser);
    }
}
