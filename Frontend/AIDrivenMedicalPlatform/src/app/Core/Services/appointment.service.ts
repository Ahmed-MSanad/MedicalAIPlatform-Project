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

    return this._http.get(`${environment.apiBaseURL}/api/appointments/doctors`, { params });
  }

  GetDoctorInfo(id: string) {
    return this._http.get(`${environment.apiBaseURL}/api/appointments/doctors/${id}`);
  }

  AddRate(doctorId: string, appointmentId: number, rate: number) {
    return this._http.patch(
      `${environment.apiBaseURL}/api/appointments/${appointmentId}/rating`,
      {
        doctorId,
        rate
      }
    );
  }


  CreateAppointment(appointment: any) {
    return this._http.post(`${environment.apiBaseURL}/api/appointments`, appointment);
  }

  CancelAppointment(appointmentId: number) {
    return this._http.delete(`${environment.apiBaseURL}/api/appointments/${appointmentId}`);
  }

  CompleteAppointment(appointmentId: number) {
    return this._http.patch(
      `${environment.apiBaseURL}/api/appointments/${appointmentId}/complete`,
      {}
    );
  }

  GetAppointments(status: number): Observable<any> {
    const params = new HttpParams().set('status', status.toString());
    return this._http.get<any>(`${environment.apiBaseURL}/api/appointments`, { params });
  }

  GetAppointmentInfo(id: number) {
    return this._http.get(`${environment.apiBaseURL}/api/appointments/${id}`);
  }

  getAvailableTimeSlots(id: string, day: Date) {
    const params = new HttpParams().set('day', day.toString());

    return this._http.get(
      `${environment.apiBaseURL}/api/appointments/doctors/${id}/available-slots`,
      { params }
    );
  }

}
