import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { TranslationService } from '../../../Core/Services/translation.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink, TranslateModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly _FormBuilder = inject(FormBuilder);
  private readonly _auth = inject(AuthService);
  private readonly _router = inject(Router);
  private readonly _toastr = inject(ToastrService);
  private readonly _translate = inject(TranslationService);
  private readonly translation = inject(TranslateService);
  loginInErrorMessage: string = "";
  currentLanguage: string = 'en';
  showLang: boolean = false;
  isLoading: boolean = false;

loginForm = this._FormBuilder.group({
  email: ['', [Validators.required, Validators.email]],
  password: ['', [Validators.required]]
});

ngOnInit() {
  if (localStorage.getItem('lang') == 'ar') {
    this.currentLanguage = 'ar';
  }
}

onLogin() {
  if (this.loginForm.valid) {
    console.log(this.loginForm.value);
    this.isLoading = true;
    this._auth.signIn(this.loginForm.value).subscribe({
      next: (res: any) => {
        console.log(res);
        this._auth.saveToken(res.token);
        const userClaims = this._auth.getClaims();
        this.isLoading = false;
        this._router.navigate(['/' + userClaims.role + "Dashboard"]);
      },
      error: (error) => {
        if (error.status == 400) {
          this._toastr.error(error.error.message, this.translation.instant('login.Login Failed'));
          this.loginInErrorMessage = error.error.message;
          console.log(error.error.message);
        }
        else {
          this._toastr.error(this.translation.instant('login.Error during login !!'), this.translation.instant('login.Login Failed'));
          this.loginInErrorMessage = this.translation.instant('login.Error during login !!');
          console.log('Error during login !!');
        }
        this.isLoading = false;
      }
    });
  }
  else {
    this.loginForm.markAllAsTouched();
  }
}

showPasswordOffOn: boolean = false;
togglePassword() {
  this.showPasswordOffOn = !this.showPasswordOffOn;
}

toggleLanguage(){
  this.showLang = !this.showLang;
}

switchLanguage(lang: string) {
  this.currentLanguage = lang;
  this._translate.changeLang(lang);
}

}
