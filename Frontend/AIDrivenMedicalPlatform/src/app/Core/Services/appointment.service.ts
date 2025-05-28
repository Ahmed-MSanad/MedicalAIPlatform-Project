import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  constructor(private _http : HttpClient) { }
  
   GetDoctors(Name?: string, Speciality?: string, MinRate?: number, Cost?: number, Workplace?: string) {
    let params = new HttpParams();

    if (Name) params = params.set('Name', Name);
    if (Speciality) params = params.set('Speciality', Speciality);
    if (MinRate) params = params.set('MinRate', MinRate);
    if (Cost) params = params.set('Cost', Cost);
    if (Workplace) params = params.set('Workplace', Workplace);

    return this._http.get(`${environment.apiBaseURL}/Appointment/GetDoctorsInfo`, { params });
  }

  GetDoctorInfo(id:string){
    let params = new HttpParams();
    if(id) params = params.set('id',id);
    
    return this._http.get(`${environment.apiBaseURL}/Appointment/GetDoctorInfo`, { params });
  }

  AddRate(id:string){
    let params = new HttpParams();
    if(id) params = params.set('id',id);
    
    return this._http.put(`${environment.apiBaseURL}/Appointment/AddRate`, { params });
  }

  CreateAppointment(appointment:any){
    return this._http.post(`${environment.apiBaseURL}/Appointment/CreateAppointment`, appointment);
  }

  CancelAppointment(appointmentId:number){
    let params = new HttpParams();
    if(appointmentId) params = params.set('appointmentId',appointmentId);
    
    return this._http.patch(`${environment.apiBaseURL}/Appointment/CancelAppointment`, { params });
  }

  CompleteAppointment(appointmentId:number){
    let params = new HttpParams();
    if(appointmentId) params = params.set('appointmentId',appointmentId);
    
    return this._http.patch(`${environment.apiBaseURL}/Appointment/CompleteAppointment`, { params });
  }

  GetAppointments(status:number){
    return this._http.get(`${environment.apiBaseURL}/Appointment/GetAppointments`,{params:{status:status}});
  }

  GetAppointmentInfo(id:number){
    let params = new HttpParams();
    if(id) params = params.set('id',id);

    return this._http.get(`${environment.apiBaseURL}/Appointment/GetAppointmentInfo`,{params});
  }

  getAvailableTimeSlots(id: string, day: Date) {
    let params = new HttpParams().set('id', id)
                                 .set('day',day.toString());

    return this._http.get(`${environment.apiBaseURL}/Appointment/GetAvailableTimeSlots`, { params });
  }

}
