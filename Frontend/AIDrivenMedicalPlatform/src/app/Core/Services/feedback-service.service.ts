import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FeedbackServiceService {

  constructor(private http: HttpClient) { }

  patientFeedback(patientFeedback : any) {
    return this.http.post(environment.apiBaseURL+"/Feedback/patientFeedback", patientFeedback);
  }

  adminFeedbackResponse(adminResponse : any) {
    return this.http.put(environment.apiBaseURL+"/Feedback/adminResponse", adminResponse);
  }

  getAllFeedbacks(){
    return this.http.get(environment.apiBaseURL+"/Feedback/getPatientFeedBacks");
  }

  removeFeedback(feedbackId : number){
    return this.http.delete(environment.apiBaseURL+`/Feedback/removeFeedBack/${feedbackId}`);
  }
}
