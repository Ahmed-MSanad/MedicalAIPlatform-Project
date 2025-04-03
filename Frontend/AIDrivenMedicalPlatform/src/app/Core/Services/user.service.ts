import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _http : HttpClient){}

  getUserProfile(){
    return this._http.get(environment.apiBaseURL+'/profile');
  }
  updateUserProfile(updatedUser: any) {
    return this._http.put(environment.apiBaseURL + '/profile', updatedUser);
  }
  deleteUserProfile() {
    return this._http.delete(environment.apiBaseURL + '/profile');
  }


}
