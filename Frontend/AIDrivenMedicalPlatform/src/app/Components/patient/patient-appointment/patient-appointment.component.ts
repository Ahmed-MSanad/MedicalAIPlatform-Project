import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { Doctor } from '../../../Core/Interfaces/doctor-card';
import { AppointmentService } from '../../../Core/Services/appointment.service';
import { FormsModule } from '@angular/forms';
import { DoctorInfo } from '../../../Core/Interfaces/doctor-info';
import { ToastrService } from 'ngx-toastr';
import { NotificationService } from '../../../Core/Services/notification.service';
import { ENotificationType } from '../../../Core/Enums/enotification-type';
import { BackgroundLayoutComponent } from "../../../Layouts/background-layout/background-layout.component";
import { MedicalImageService } from '../../../Core/Services/medical-image.service';

@Component({
  selector: 'app-patient-appointment',
  imports: [FormsModule, BackgroundLayoutComponent],
  templateUrl: './patient-appointment.component.html',
  styleUrl: './patient-appointment.component.scss'
})
export class PatientAppointmentComponent {

  private _appointmentService = inject(AppointmentService);
  private _medicalImageService = inject(MedicalImageService);
  private _toastr = inject(ToastrService);

  doctors: Doctor[] = [];
  error: string | null = null;
  selectedDoctor!: DoctorInfo;
  showModal = false;
  isLoading = false;
  filters = {
    name: '',
    speciality: '',
    minRate: undefined as number | undefined,
    cost: undefined as number | undefined,
    workplace: ''
  };
  date?: Date
  time?: string
  description?: string
  id!: string
  timeSlots!: string[]
  minDate!: string;
  maxDate!: string;

  @ViewChild('fileInput') fileInput!: ElementRef;
  imageSrc: string = '';
  medicalImage: string | null = null;




  ngOnInit() {
    this.isLoading = true;
    this.searchDoctors();
    this.setDateConstraints();
  }

  searchDoctors() {
    this._appointmentService.GetDoctors(this.filters.name, this.filters.speciality, this.filters.minRate, this.filters.cost, this.filters.workplace).subscribe({
      next: (res: any) => {
        this.doctors = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.log(err);
        this.isLoading = false;
      }
    })
  }


  clearFilters() {
    this.filters = {
      name: '',
      speciality: '',
      minRate: undefined,
      cost: undefined,
      workplace: ''
    };
    this.doctors = [];
    this.error = null;

    this.searchDoctors();
  }

  closeModal() {
    this.showModal = false;
    this.date = undefined;
    this.time = undefined;
    this.description = undefined;
    this.imageSrc = ''
    this.medicalImage = null
  }

  openModal(id: string) {
    this.isLoading = true;
    this._appointmentService.GetDoctorInfo(id).subscribe({
      next: (res: any) => {
        this.selectedDoctor = res
        this.id = id
        this.showModal = true;
        this.isLoading = false;
      },
      error: (err) => {
        console.log(err);
        this.isLoading = false;
      }
    })
  }

  getImageUrl(imageData: string): string {
    return `data:image/jpeg;base64,${imageData}`;
  }


  getTimeSlots() {
    this.timeSlots = [];
    if (this.date) {
      this._appointmentService.getAvailableTimeSlots(this.id, this.date).subscribe({
        next: (res: any) => {
          this.timeSlots = res
        },
        error: (err) => {
          console.log(err)
        }
      })
    }
  }

  setDateConstraints() {
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];

    const futureDate = new Date();
    futureDate.setDate(today.getDate() + 30);
    this.maxDate = futureDate.toISOString().split('T')[0];
  }


  notificationService = inject(NotificationService);
  bookAppointment() {

    if (this.time && this.date) {

      const localDate = new Date(this.date);
      const [hours, minutes] = this.time.split(':').map(Number);
      localDate.setHours(hours, minutes);
      const gmtOffsetMinutes = localDate.getTimezoneOffset();
      const gmtOffsetHours = gmtOffsetMinutes / 60;
      localDate.setHours(
        localDate.getHours() - Math.sign(gmtOffsetHours) * Math.floor(Math.abs(gmtOffsetHours)),
        localDate.getMinutes() - (Math.abs(gmtOffsetMinutes)) % 60
      );

      this._appointmentService.CreateAppointment({
        Date: localDate,
        Cost: this.selectedDoctor.fee,
        Location: this.selectedDoctor.workPlace,
        Description: this.description,
        Did: this.id
      }).subscribe({
        next: (res: any) => {
          this._toastr.success(res.message);
          this.showModal = false;
          this.time = undefined;
          this.date = undefined;
          this.description = undefined;
          this.notificationService.sendNotification(ENotificationType.Success).subscribe({
            next: (res) => {
              this._toastr.success(res.message);
              console.log(res.message);
            },
            error: (error) => {
              this._toastr.error(error.error);
              console.log(error.error);
            }
          });
          if (this.medicalImage != null) {
            this.UploadImage(res)
          }
        },
        error: (err) => {
          this._toastr.error(err.message);
        }
      })
    }
    else {
      this._toastr.error("Please Select Date and Time");
    }
  }


  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }

  ChangeImage(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.[0]) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imageSrc = reader.result as string;
        const base64 = (reader.result as string).split(',')[1];
        this.medicalImage = base64;
      };
      reader.readAsDataURL(input.files[0]);
    }
  }

  removeMedicalImage() {
    this.medicalImage = null;
  }


  UploadImage(res:any) {
    this._medicalImageService.CreateMedicalImage({
      Image: this.medicalImage,
      Did: this.id,
      AppointmentId: res.appointmentId
    }).subscribe({
      next:(res:any)=>{
        console.log(res.message);
        this.imageSrc = ''
        this.medicalImage = null
      },
      error:(err)=>{
        this._toastr.error("Couldn't Add Image")
        this.imageSrc = ''
        this.medicalImage = null
      }
    })
  }
}
