import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _http : HttpClient){}

  deleteUserProfile() {
    return this._http.delete(environment.apiBaseURL + '/Profile/DeleteUserProfile');
  }

}
