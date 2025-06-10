import { FormBuilder } from '@angular/forms';
import { DatePipe, NgClass } from '@angular/common';
import { IFeedback } from '../../Core/Interfaces/ifeedback';
import { FeedbackServiceService } from './../../Core/Services/feedback-service.service';
import { Component, inject, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../Core/Services/auth.service';
import { claimReq } from '../../Core/utils/claimReq-utils';
import { HideIfClaimsNotMetDirective } from '../../Core/directives/hide-if-claims-not-met.directive';
import { BackgroundLayoutComponent } from "../../Layouts/background-layout/background-layout.component";

@Component({
  selector: 'app-feedback',
  imports: [NgClass, DatePipe, HideIfClaimsNotMetDirective, BackgroundLayoutComponent],
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.scss'
})
export class FeedbackComponent implements OnInit{

  constructor(private feedbackService : FeedbackServiceService){}

  feedbacks : IFeedback[] = [];

  getRatingColor(rating: number): string {
    if (rating >= 5) return 'text-emerald-500';
    if (rating >= 4) return 'text-green-500';
    if (rating >= 3) return 'text-yellow-500';
    if (rating >= 2) return 'text-orange-500';
    return 'text-red-500';
  }

  getRatingBg(rating: number): string {
    if (rating >= 5) return 'bg-emerald-50 border-emerald-200';
    if (rating >= 4) return 'bg-green-50 border-green-200';
    if (rating >= 3) return 'bg-yellow-50 border-yellow-200';
    if (rating >= 2) return 'bg-orange-50 border-orange-200';
    return 'bg-red-50 border-red-200';
  }

  getBorderColor(rating: number): string {
    if (rating >= 5) return 'border-l-emerald-500';
    if (rating >= 4) return 'border-l-green-500';
    if (rating >= 3) return 'border-l-yellow-500';
    if (rating >= 2) return 'border-l-orange-500';
    return 'border-l-red-500';
  }

  getStarArray(rating: number): boolean[] {
    return Array.from({ length: 5 }, (_, index) => index < rating);
  }

  getTotalReviews(): number {
    return this.feedbacks.length;
  }

  getAverageRating(): string {
    if(this.feedbacks.length > 0){
      const average = this.feedbacks.reduce((sum, f) => sum + f.rating, 0) / (this.feedbacks.length * 5);
      return average.toFixed(1);
    }
    else{
      return "0";
    }
  }

  getResponseRate(): number {
    if(this.feedbacks.length > 0){
      const respondedCount = this.feedbacks.filter(f => f.responseMessage).length;
      return Math.round((respondedCount / this.feedbacks.length) * 100);
    }
    else{
      return 0;
    }
  }

  // Get Patient Feedback:
  private readonly formBuilder = inject(FormBuilder);
  private readonly toastr = inject(ToastrService);
  sendPatientFeedback(){

    Swal.fire({
      title: "Patient Feedback Form",
      html: `
        <form>
            <div class="space-y-4">
              <div>
                <label for="swal-feedbackMessage">Message:</label>
                <textarea id="swal-feedbackMessage" name="feedback" rows="5" cols="30" class="border-2 p-4 rounded-lg"></textarea>
              </div>
              <div>
                <label for="swal-feedbackRating">Rating:</label>
                <input id="swal-feedbackRating" type="number" class="border-2 p-4 rounded-lg" min="0" max="5"/>
              </div>
            </div>
        </form>
      `,
      showCancelButton: true,
      confirmButtonText: "Go ahead 😃",
      cancelButtonText: "Cancel",
      focusConfirm: false,
      preConfirm:() => {
        const message = (document.getElementById("swal-feedbackMessage") as HTMLTextAreaElement)?.value;
        const rating = parseInt((document.getElementById("swal-feedbackRating") as HTMLInputElement)?.value);

        if(!message){
          Swal.showValidationMessage("Message is required to continue !");
          setTimeout(() => {
            Swal.resetValidationMessage();
          }, 2000);
          return false;
        }
        if(!rating){
          Swal.showValidationMessage("Rating is required to continue !");
          setTimeout(() => {
            Swal.resetValidationMessage();
          }, 2000);
          return false;
        }

        return {message, rating};
      }
    }).then((result) =>{
      let errorMessage = "";
      if(result.isConfirmed){
        let patientFeedbackForm = this.formBuilder.group({
          message : [result.value.message],
          rating : [result.value.rating]
        });
        
        // console.log(patientFeedbackForm.value);

        this.feedbackService.patientFeedback(patientFeedbackForm.value).subscribe({
          next:(res : any) => {
            // console.log(res);
            Swal.fire('Success!', 'Feedback is Created successfully', 'success');
            this.feedbacks = [res, ...this.feedbacks];
          },
          error: (error) => {
            this.toastr.error(error.error.message, 'Submit feedback Failed');
            errorMessage = error.error.message;
            // console.log(error.error.message);

            if(errorMessage)
              Swal.fire('Error!', errorMessage, 'error');
          }
        });
      }
    });

  }

  // Get User Role:
  private readonly _auth = inject(AuthService);
  userClaims : any;
  claimReq = claimReq;

  ngOnInit(): void {
    this.feedbackService.getAllFeedbacks().subscribe({
      next: (res : any) => {
        this.feedbacks = res;
        // console.log(this.feedbacks);
        this.feedbacks = this.feedbacks.reverse();
      },
      error: (err) => {
        console.log(err);
      }
    });

    this.userClaims = this._auth.getClaims();
    // console.log(this.userClaims);
  }

  // Admin Response:
  sendAdminRespond(IdOfFeedback : number){

    Swal.fire({
      title: "Patient Feedback Form",
      html: `
        <form>
            <label for="swal-responseMessage">Message:</label>
            <textarea id="swal-responseMessage" name="feedback" rows="5" cols="30" class="border-2 p-4 rounded-lg"></textarea>
        </form>
      `,
      showCancelButton: true,
      confirmButtonText: "Go ahead 😃",
      cancelButtonText: "Cancel",
      focusConfirm: false,
      preConfirm:() => {
        const responseMessage = (document.getElementById("swal-responseMessage") as HTMLTextAreaElement)?.value;

        if(!responseMessage){
          Swal.showValidationMessage("Message is required to continue !");
          setTimeout(() => {
            Swal.resetValidationMessage();
          }, 2000);
          return false;
        }

        return {responseMessage};
      }
    }).then((result) =>{
      let errorMessage = "";
      if(result.isConfirmed){
        let adminResponseForm = this.formBuilder.group({
          feedbackId : IdOfFeedback,
          responseMessage : [result.value.responseMessage]
        });
        
        // console.log(adminResponseForm.value);

        this.feedbackService.adminFeedbackResponse(adminResponseForm.value).subscribe({
          next:(res : any) => {
            console.log(res);
            Swal.fire('Success!', "Admin Response is done successfully", 'success');
            const index = this.feedbacks.findIndex(f => f.feedbackId === IdOfFeedback);
            if (index !== -1) this.feedbacks[index] = res; // Replace at the same index
          },
          error: (error) => {
            this.toastr.error(error.error.message, 'Submit feedback Failed');
            errorMessage = error.error.message;
            console.log(error.error.message);

            if(errorMessage)
              Swal.fire('Error!', errorMessage, 'error');
          }
        });
      }
    });

  }

  // Admin & Right Patient removes feedback:
  removeFeedback(feedbackId : number){
    this.feedbackService.removeFeedback(feedbackId).subscribe({
      next: (res:any) => {
        this.toastr.success(res.message);
        this.feedbacks = this.feedbacks.filter(f => f.feedbackId !== feedbackId);
      },
      error:(err) =>{
        // console.log(err.error.error);
        let errorMessage = err.error.error;
        Swal.fire({
          icon: 'error',
          title: 'Not Your feedback!!',
          text: errorMessage,
        });
      }
    });
  }

}
