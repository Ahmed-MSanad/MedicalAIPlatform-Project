import { Component, inject} from '@angular/core';
import { DoctorService } from '../../../Core/Services/ForDoctor/doctor.service';
import { DayInSchedule } from '../../../Core/Interfaces/day-in-schedule';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [],
  templateUrl: './doctor-dashboard.component.html',
  styleUrl: './doctor-dashboard.component.scss'
})
export class DoctorDashboardComponent {
  
  private _schedule = inject(DoctorService)
  
  daysOfWeek = ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday']
  days:DayInSchedule[] = [];
  editSchedule:boolean = false;

  ngOnInit(){
    this._schedule.getDoctorSchedule().subscribe({
      next:(res:any)=>{
        this.days = res;
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }

  ToggleSchedule(){
    this.editSchedule = !this.editSchedule;
  }

  SaveSchedule() {
    // const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    // this.invalidSchedule=false;
    // const updatedSchedules: any = {};

    // days.forEach(day => {
    //   const isChecked = (document.getElementById(day) as HTMLInputElement).checked;
    //   const fromTime = (document.getElementById(`${day}from`) as HTMLInputElement).value;
    //   const toTime = (document.getElementById(`${day}to`) as HTMLInputElement).value;

    //   if (isChecked) {
    //     if(!fromTime||!toTime){
    //       this.invalidSchedule = true;
    //     }
    //     updatedSchedules[day] = { from: fromTime, to: toTime };
    //   } else {
    //     updatedSchedules[day] = { from: '--:--', to: '--:--' };
    //   }
    //   this.profileForm.get('schedules')?.setValue(updatedSchedules); 
    // });

    // if(!this.invalidSchedule){
    // this.editSchedule = false;
    // }
  }
}
