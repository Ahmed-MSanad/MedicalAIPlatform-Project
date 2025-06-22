import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { error } from 'console';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { TranslationService } from '../../../Core/Services/translation.service';

@Component({
  selector: 'app-forget-password',
  imports: [ReactiveFormsModule, CommonModule, RouterLink, TranslateModule],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.scss'
})
export class ForgetPasswordComponent {
  forgotPasswordForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  currentLanguage: string = 'en';
  showLang: boolean = false;
  private readonly authService = inject(AuthService);
  private readonly toastr = inject(ToastrService);
  private readonly translate = inject(TranslateService);
  private readonly _translate = inject(TranslationService);

  constructor(private formBuilder: FormBuilder) {
    this.forgotPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }
  ngOnInit() {
    if (localStorage.getItem('lang') == 'ar') {
      this.currentLanguage = 'ar';
    }
  }
  onSubmit() {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      this.authService.forgetPassword(this.forgotPasswordForm.value).subscribe({
        next: (res: any) => {
          console.log(res);
          this.successMessage = res.message;
          this.toastr.success(this.successMessage, this.translate.instant("forget.Reset Email is sent"));
          this.isLoading = false;
        },
        error: (err) => {
          console.log(err.error);
          this.errorMessage = err.error.error;
          this.toastr.error(this.errorMessage, this.translate.instant("forget.Error !!"));
          this.isLoading = false;
        }
      });
    }
  }

  toggleLanguage() {
    this.showLang = !this.showLang;
  }

  switchLanguage(lang: string) {
    this.currentLanguage = lang;
    this._translate.changeLang(lang);
  }
}
