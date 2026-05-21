import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private http: HttpClient) { }

  sendNotification(notificationType: any): Observable<any> {
    const params = new HttpParams().set('notificationType', notificationType);

    return this.http.post(
      `${environment.apiBaseURL}/api/notifications/send`,
      {},
      { params }
    );
  }

  getPatientNotifications(): Observable<any> {
    return this.http.get(
      `${environment.apiBaseURL}/api/notifications`
    );
  }

  removeNotification(notificationId: number): Observable<any> {
    return this.http.delete(
      `${environment.apiBaseURL}/api/notifications/${notificationId}`
    );
  }
}
