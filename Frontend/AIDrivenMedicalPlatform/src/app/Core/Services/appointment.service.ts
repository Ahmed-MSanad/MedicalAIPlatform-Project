import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  constructor(private _http: HttpClient) { }

  GetDoctors(Name?: string, Speciality?: string, MinRate?: number, Cost?: number, Workplace?: string) {
    let params = new HttpParams();

    if (Name) params = params.set('Name', Name);
    if (Speciality) params = params.set('Speciality', Speciality);
    if (MinRate) params = params.set('MinRate', MinRate);
    if (Cost) params = params.set('Cost', Cost);
    if (Workplace) params = params.set('Workplace', Workplace);

    return this._http.get(`${environment.apiBaseURL}/Appointment/GetDoctorsInfo`, { params });
  }

  GetDoctorInfo(id: string) {
    let params = new HttpParams();
    if (id) params = params.set('id', id);

    return this._http.get(`${environment.apiBaseURL}/Appointment/GetDoctorInfo`, { params });
  }

  AddRate(id: string, appointmentId: number, rate: number) {
    let params = new HttpParams();
    if (id) params = params.set('id', id);
    if (appointmentId) params = params.set('appointmentId', appointmentId.toString());
    if (rate) params = params.set('rate', rate.toString());

    return this._http.patch(`${environment.apiBaseURL}/Appointment/AddRate`, null, { params });
  }


  CreateAppointment(appointment: any) {
    return this._http.post(`${environment.apiBaseURL}/Appointment/CreateAppointment`, appointment);
  }

  CancelAppointment(appointmentId: number) {
    let params = new HttpParams();
    if (appointmentId) params = params.set('appointmentId', appointmentId);

    return this._http.delete(`${environment.apiBaseURL}/Appointment/CancelAppointment`, { params });
  }

  CompleteAppointment(appointmentId: number) {
    let params = new HttpParams();
    if (appointmentId) params = params.set('appointmentId', appointmentId);

    return this._http.patch(`${environment.apiBaseURL}/Appointment/CompleteAppointment`, { params });
  }

  GetAppointments(status: number): Observable<any> {
    
  const params = new HttpParams().set('status', status.toString());
  return this._http.get<any>(`${environment.apiBaseURL}/Appointment/GetAppointments`, { params });
}

  GetAppointmentInfo(id: number) {
    let params = new HttpParams();
    if (id) params = params.set('id', id);

    return this._http.get(`${environment.apiBaseURL}/Appointment/GetAppointmentInfo`, { params });
  }

  getAvailableTimeSlots(id: string, day: Date) {
    let params = new HttpParams().set('id', id)
      .set('day', day.toString());

    return this._http.get(`${environment.apiBaseURL}/Appointment/GetAvailableTimeSlots`, { params });
  }

}
