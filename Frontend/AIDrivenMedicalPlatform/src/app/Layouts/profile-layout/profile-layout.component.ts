import { Component, inject } from '@angular/core';
import { UserService } from '../../Core/Services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../../Core/Services/auth.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile-layout',
  imports: [ReactiveFormsModule],
  templateUrl: './profile-layout.component.html',
  styleUrl: './profile-layout.component.scss'
})
export class ProfileLayoutComponent {

  
  
}
