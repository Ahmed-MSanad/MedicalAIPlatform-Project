import { Component, inject } from '@angular/core';
import { AppointmentService } from '../../../Core/Services/appointment.service';
import { RouterModule } from '@angular/router';
import { Appointment } from '../../../Core/Interfaces/appointment';
import { CarouselModule, OwlOptions } from 'ngx-owl-carousel-o';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AppointmentInfo } from '../../../Core/Interfaces/appointment-info';
import { BackgroundLayoutComponent } from "../../../Layouts/background-layout/background-layout.component";
@Component({
  selector: 'app-patient-dashboard',
  imports: [RouterModule, CarouselModule, DatePipe, BackgroundLayoutComponent],
  templateUrl: './patient-dashboard.component.html',
  styleUrl: './patient-dashboard.component.scss'
})
export class PatientDashboardComponent {

  private _appointmentService = inject(AppointmentService);
  private _toastr = inject(ToastrService)


  customOptions: OwlOptions = {
    loop: true,
    mouseDrag: false,
    touchDrag: false,
    pullDrag: false,
    dots: false,
    navSpeed: 700,
    navText: ['', ''],
    responsive: {
      0: {
        items: 1
      }
    },
    nav: true
  }

  isLoading = false;
  isDelete = false;
  id?:number;
  doctorId?:string;
  appointments!:Appointment[];
  appointmentInfo!:AppointmentInfo;
  showInfo = false;
  showRating = false;
  status:number = 0;
  rating = 0;
  
  ngOnInit(){
    this.GetAppointments();
  }

  GetAppointments(){
    this.isLoading = true;
    this._appointmentService.GetAppointments(this.status).subscribe({
      next:(res:any)=>{
        this.appointments = res;
        console.log(this.appointments);
        
        this.isLoading = false;
      },
      error:(err)=>{
        console.log(err);
        this.isLoading = false;
      }
    })
  }

  OpenDelete(event:Event,id:number){
    event.stopPropagation();
    this.isDelete = true;
    this.id = id
  }
  CloseDelete(){
    this.isDelete = false;
  }
  SelectStatus(status:number) {
    if(this.status != status){
      this.status = status;
      this.GetAppointments();
    }
  }
  CancelAppointment(){
    this._appointmentService.CancelAppointment(this.id!).subscribe({
      next:(res:any)=>{
        this._toastr.success(res.message);
        this.appointments = this.appointments.filter(a => a.id !== this.id);
        this.id = undefined;
        this.CloseDelete();
      },
      error:(err)=>{
        this._toastr.error(err.message);
      }
    })
  }
  ShowAppointmentInfo(id:number){
    this.isLoading = true;
    this._appointmentService.GetAppointmentInfo(id).subscribe({
      next:(res:any)=>{
        this.appointmentInfo = res;
        this.isLoading = false;
        this.showInfo = true;
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }

  CloseInfo(){
    this.showInfo = false;
  }
  CloseRating(){
    this.showRating = false;
    this.id = undefined;
    this.doctorId = undefined;
    this.rating = 0;
  }
  OpenRating(event:Event,appointmentId:number,Did:string){
    event.stopPropagation();
    this.id = appointmentId
    this.doctorId = Did;
    this.showRating = true;
  }
  SubmitRating(){
    if(this.rating >= 1 && this.rating <= 5){
      this._appointmentService.AddRate(this.doctorId!,this.id!,this.rating).subscribe({
        next:(res:any)=>{
          this._toastr.success(res.message);
          this.appointments = this.appointments.map(appointment=> appointment.id == this.id? {...appointment,isRated:true}:appointment)
          this.id = undefined;
          this.doctorId = undefined;
          this.showRating = false;
        },
        error:(err)=>{
          this._toastr.error(err.message);
        }
      })
    }
  }
  setRating(rating:number){
    this.rating = rating;
  }
}
