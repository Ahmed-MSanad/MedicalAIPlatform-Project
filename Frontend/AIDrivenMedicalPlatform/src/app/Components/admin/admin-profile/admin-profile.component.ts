import { Component, inject } from '@angular/core';
import { UserService } from '../../../Core/Services/user.service';
import { UserProfile } from '../../../Core/Interfaces/user-profile';
import { DatePipe } from '@angular/common';
import { ProfileLayoutComponent } from "../../../Layouts/profile-layout/profile-layout.component";

@Component({
  selector: 'app-admin-profile',
  imports: [DatePipe, ProfileLayoutComponent],
  templateUrl: './admin-profile.component.html',
  styleUrl: './admin-profile.component.scss'
})
export class AdminProfileComponent {
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
