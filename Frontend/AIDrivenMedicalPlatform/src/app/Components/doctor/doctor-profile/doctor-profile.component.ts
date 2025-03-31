import { DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { UserService } from '../../../Core/Services/user.service';
import { UserProfile } from '../../../Core/Interfaces/user-profile';

@Component({
  selector: 'app-doctor-profile',
  imports: [DatePipe],
  templateUrl: './doctor-profile.component.html',
  styleUrl: './doctor-profile.component.scss'
})
export class DoctorProfileComponent {
  private readonly _user = inject(UserService);

  userProfile : UserProfile = {} as UserProfile;
  ngOnInit(): void {
    this._user.getUserProfile().subscribe({
      next: (res : any) => {
        this.userProfile = res;
        console.log(this.userProfile);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
}
