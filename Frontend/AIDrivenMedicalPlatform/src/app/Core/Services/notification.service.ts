import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private http: HttpClient) { }

  sendNotification(NotificationType : any) : Observable<any>{
    return this.http.post(environment.apiBaseURL+`/Notification/SendEmail/${NotificationType}`,{});
  }

  getPatientNotifications() : Observable<any>{
    return this.http.get(environment.apiBaseURL+"/Notification/getNotifications");
  }

  removeNotification(NotificationId : number) : Observable<any>{
    return this.http.delete(environment.apiBaseURL+`/Notification/removeNotification/${NotificationId}`);
  }
}
