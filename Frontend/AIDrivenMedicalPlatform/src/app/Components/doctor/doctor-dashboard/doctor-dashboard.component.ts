import { Component, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DoctorService } from '../../../Core/Services/ForDoctor/doctor.service';
import { ToastrService } from 'ngx-toastr';
import { BackgroundLayoutComponent } from "../../../Layouts/background-layout/background-layout.component";
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-doctor-dashboard',
  templateUrl: './doctor-dashboard.component.html',
  styleUrls: ['./doctor-dashboard.component.scss'],
  imports: [RouterModule,BackgroundLayoutComponent]
})
export class DoctorDashboardComponent {

  private readonly _schedule = inject(DoctorService);
  private readonly _toastr = inject(ToastrService);


  days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

  scheduleForm: FormGroup = new FormGroup({
    schedules: new FormControl(
      this.days.map(day => ({ 
        day: day, 
        from: '', 
        to: '' 
      }))
  )});


  editSchedule: boolean = false;
  invalidSchedule: boolean = false;
  isLoading: boolean = false;

  ngOnInit() {
    this.isLoading = true;
    this._schedule.getDoctorSchedule().subscribe({
      next: (res) => {
        this.scheduleForm.get('schedules')?.setValue(res)
        console.log(res)
        console.log(this.scheduleForm.get('schedules')?.value)
        this.isLoading = false;
      },
      error: (err) => {
        console.log(err);
        this.isLoading = false;
      }
    })
  }

  ToggleEdit() {
    this.editSchedule = !this.editSchedule;
  }

  SaveSchedule() {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    this.invalidSchedule = false;
    const updatedSchedules: { day: string, from: string, to: string }[] = [];
    days.forEach(day => {
      const dayElement = document.getElementById(day) as HTMLInputElement;
      const fromElement = document.getElementById(`${day}from`) as HTMLInputElement;
      const toElement = document.getElementById(`${day}to`) as HTMLInputElement;

      const isChecked = dayElement?.checked || false;
      const fromTime = fromElement?.value || '';
      const toTime = toElement?.value || '';

      if (isChecked) {
        if (fromTime && toTime) {
          updatedSchedules.push({
            day: day,
            from: fromTime,
            to: toTime
          });
        } else {
          this.invalidSchedule = true;
        }
      }
    });    
    if (!this.invalidSchedule) {
      console.log(updatedSchedules)
      this.scheduleForm.get('schedules')?.setValue(updatedSchedules);
    this._schedule.updateDoctorSchedule(updatedSchedules).subscribe({
      next:()=>{
        this._toastr.success("Schedule Changed Successfully");
      },
      error:()=>{
        this._toastr.error("Failed To Edit");
      }
    })
      this.editSchedule = false;
    }
  }

}