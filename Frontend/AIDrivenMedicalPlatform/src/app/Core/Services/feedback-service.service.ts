import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FeedbackServiceService {

  constructor(private http: HttpClient) { }

  patientFeedback(feedback: any) {
    return this.http.post(
      `${environment.apiBaseURL}/api/feedback`,
      feedback
    );
  }

  adminFeedbackResponse(adminResponse: any) {
    return this.http.put(
      `${environment.apiBaseURL}/api/feedback/${adminResponse.feedbackId}/response`,
      adminResponse
    );
  }

  getAllFeedbacks() {
    return this.http.get(
      `${environment.apiBaseURL}/api/feedback`
    );
  }

  removeFeedback(feedbackId: number) {
    return this.http.delete(
      `${environment.apiBaseURL}/api/feedback/${feedbackId}`
    );
  }
}
