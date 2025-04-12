import { Component, inject } from '@angular/core';
import { UserService } from '../../../Core/Services/user.service';
import { UserProfile } from '../../../Core/Interfaces/user-profile';
import { ProfileLayoutComponent } from "../../../Layouts/profile-layout/profile-layout.component";


@Component({
  selector: 'app-patient-profile',
  imports: [ProfileLayoutComponent],
  templateUrl: './patient-profile.component.html',
  styleUrl: './patient-profile.component.scss'
})
export class PatientProfileComponent {
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
