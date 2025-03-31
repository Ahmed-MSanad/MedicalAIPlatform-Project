import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../Constants';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  createUser(formData : any){
    return this.http.post(environment.apiBaseURL+'/signup', formData);
  }

  checkEmail(formData : any){
    return this.http.post(environment.apiBaseURL+'/usercheck/check-email', formData);
  }

  signIn(formData : any){
    return this.http.post(environment.apiBaseURL+'/signin' ,formData);
  }

  isLoggedIn() : boolean{
    return this.getToken() !== null;
  }

  saveToken(token : string){
    localStorage.setItem(TOKEN_KEY, token);
  }

  isValidLocalStorage(){
    return typeof localStorage !== 'undefined';
  }

  getToken(){
    if(this.isValidLocalStorage()){
      return localStorage.getItem(TOKEN_KEY);
    }
    else{
      return "";
    }
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }

  getClaims(){
    return JSON.parse(window.atob(this.getToken()!.split('.')[1])); // token -> header.payload.signature
  }
}
