import { Component, inject } from '@angular/core';
import { AppointmentService } from '../../../Core/Services/appointment.service';
import { Appointment } from '../../../Core/Interfaces/appointment';
import { AppointmentInfo } from '../../../Core/Interfaces/appointment-info';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { BackgroundLayoutComponent } from "../../../Layouts/background-layout/background-layout.component";
@Component({
  selector: 'app-doctor-appointments',
  imports: [DatePipe, FormsModule, BackgroundLayoutComponent],
  templateUrl: './doctor-appointments.component.html',
  styleUrl: './doctor-appointments.component.scss'
})
export class DoctorAppointmentsComponent {

  private _appointmentService = inject(AppointmentService);
  private _toastr = inject(ToastrService);

  isLoading = false;
  appointments!: Appointment[]
  status: number = 0
  isDelete = false;
  id?: number;
  appointmentInfo!: AppointmentInfo;
  showInfo = false;
  patientName = "";
  ngOnInit() {
    this.GetAppointments();
  }

  get filteredAppointments(): Appointment[] {
  if (!this.patientName || this.patientName.trim() === "") return this.appointments;
  return this.appointments.filter(a =>
    a.patientName.toLowerCase().includes(this.patientName.toLowerCase())
  );
}


  GetAppointments() {
    this.isLoading = true;
    this._appointmentService.GetAppointments(this.status).subscribe({
      next: (res: any) => {
        this.appointments = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.log(err.message);
        this.isLoading = false;
      }
    })
  }

  SelectStatus(status: number) {
    if (this.status != status) {
      this.status = status;
      this.patientName = "";
      this.GetAppointments();
    }
  }

  OpenDelete(event: Event, id: number) {
    event.stopPropagation();
    this.isDelete = true;
    this.id = id
  }
  CloseDelete() {
    this.isDelete = false;
  }


  ShowAppointmentInfo(id: number) {
    this.isLoading = true;
    this._appointmentService.GetAppointmentInfo(id).subscribe({
      next: (res: any) => {
        this.appointmentInfo = res;
        this.isLoading = false;
        this.showInfo = true;
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  CloseInfo() {
    this.showInfo = false;
    this.id = undefined;
  }

  CancelAppointment() {
    this._appointmentService.CancelAppointment(this.id!).subscribe({
      next: (res: any) => {
        this._toastr.success(res.message);
        this.appointments = this.appointments.filter(a => a.id !== this.id);
        this.id = undefined;
        this.CloseDelete();
      },
      error: (err) => {
        this._toastr.error(err.message);
      }
    })
  }

  CompleteAppointment(event: Event, appointmentId: number) {
    event.stopPropagation();

    this.isLoading = true;
    this._appointmentService.CompleteAppointment(appointmentId).subscribe({
      next: (res: any) => {
        this._toastr.success(res.message);
        this.appointments = this.appointments.filter(appointment => appointment.id != appointmentId)
        this.isLoading = false;
      },
      error: (err) => {
        this._toastr.error("Can't complete this appointment until the scheduled time has passed.");
        this.isLoading = false;
      }
    })
  }
}
