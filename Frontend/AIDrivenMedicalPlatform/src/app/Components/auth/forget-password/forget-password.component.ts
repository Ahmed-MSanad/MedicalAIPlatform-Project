import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { error } from 'console';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forget-password',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.scss'
})
export class ForgetPasswordComponent {
  forgotPasswordForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  private readonly authService = inject(AuthService);
  private readonly toastr = inject(ToastrService)

  constructor(private formBuilder: FormBuilder) {
    this.forgotPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit() {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      this.authService.forgetPassword(this.forgotPasswordForm.value).subscribe({
        next:(res : any) => {
          console.log(res);
          this.successMessage = res.message;
          this.toastr.success(this.successMessage,"Reset Email is sent");
          this.isLoading = false;
        },
        error:(err) => {
          console.log(err.error);
          this.errorMessage = err.error.error;
          this.toastr.error(this.errorMessage,"Error !!");
          this.isLoading = false;
        }
      });
    }
  }
}
