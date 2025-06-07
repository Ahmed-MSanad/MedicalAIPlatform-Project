import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly _FormBuilder = inject(FormBuilder);
  private readonly _auth = inject(AuthService);
  private readonly _router = inject(Router);
  private readonly _toastr = inject(ToastrService);
  loginInErrorMessage : string = "";

  loginForm = this._FormBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  onLogin(){
    if(this.loginForm.valid){
      console.log(this.loginForm.value);
      this._auth.signIn(this.loginForm.value).subscribe({
        next:(res : any) => {
          console.log(res);
          this._auth.saveToken(res.token);
          const userClaims = this._auth.getClaims();
          this._router.navigate(['/'+userClaims.role+"Dashboard"]);
        },
        error: (error) => {
          if(error.status == 400){
            this._toastr.error(error.error.message, 'Login Failed');
            this.loginInErrorMessage = error.error.message;
            console.log(error.error.message);
          }
          else{
            this._toastr.error('Error during login !!', 'Login Failed');
            this.loginInErrorMessage = 'Error during login !!';
            console.log('Error during login !!');
          }
        }
      });
    }
    else{
      this.loginForm.markAllAsTouched();
    }
  }

  showPasswordOffOn : boolean = false;
  togglePassword(){
    this.showPasswordOffOn = !this.showPasswordOffOn;
  }
}
