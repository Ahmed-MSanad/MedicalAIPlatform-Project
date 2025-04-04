import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  constructor(private _http:HttpClient) { }

  getDoctorSchedule(){
    return this._http.get(environment.apiBaseURL + '/schedule')
  }
  updateDoctorSchedule(updatedSchedule: any){
    return this._http.put(environment.apiBaseURL + '/schedule', updatedSchedule)
  }
}
