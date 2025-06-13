import { Component, inject } from '@angular/core';
import { AppointmentService } from '../../../Core/Services/appointment.service';
import { Appointment } from '../../../Core/Interfaces/appointment';
import { AppointmentInfo } from '../../../Core/Interfaces/appointment-info';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { BackgroundLayoutComponent } from "../../../Layouts/background-layout/background-layout.component";
import { MedicalImageService } from '../../../Core/Services/medical-image.service';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-doctor-appointments',
  imports: [DatePipe, FormsModule, BackgroundLayoutComponent],
  templateUrl: './doctor-appointments.component.html',
  styleUrl: './doctor-appointments.component.scss'
})
export class DoctorAppointmentsComponent {

constructor(private http: HttpClient) {}


  private _appointmentService = inject(AppointmentService);
  private _medicalImageService = inject(MedicalImageService)
  private _toastr = inject(ToastrService);

  isLoading = false;
  appointments!: Appointment[]
  status: number = 0
  isDelete = false;
  id?: number;
  appointmentInfo!: AppointmentInfo;
  showInfo = false;
  patientName = "";
  imageSrc: string = '';
  medicalImage: string | null = null;
  medicalImageId !: number

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
        this.id = id
        this.getMedicalImage(id);
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

  getMedicalImage(appointmentId: number) {
    this.isLoading = true;
    this._medicalImageService.GetMedicalImage(appointmentId).subscribe({
      next: (res: any) => {
        console.log(res);
        this.medicalImageId = res.medicalImageId;
        this.medicalImage = res.image;
        this.imageSrc = `data:image/png;base64,${this.medicalImage}`;
        this.isLoading = false;
        this.showInfo = true;
      },
      error: (err) => {
        console.log(err);
        this.isLoading = false;
        this.showInfo = true;
      }
    })
  }


  upload(){
    
const blob = this.base64ToBlob(this.imageSrc, 'image/png');
    const file = new File([blob], 'image.png', { type: 'image/png' });

    const formData = new FormData();
    formData.append('file', file);

    this.http.post('http://127.0.0.1:8000/upload-image/', formData).subscribe({
      next: (res) => {
        console.log('Success:', res);
      },
      error: (err) => {
        console.error('Error:', err);
      }
    });
  }

  base64ToBlob(base64: string, contentType = 'image/png'): Blob {
    const byteCharacters = atob(base64.split(',')[1]);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      const slice = byteCharacters.slice(offset, offset + 512);
      const byteNumbers = new Array(slice.length);

      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
        return new Blob(byteArrays, { type: contentType });
  }

}

