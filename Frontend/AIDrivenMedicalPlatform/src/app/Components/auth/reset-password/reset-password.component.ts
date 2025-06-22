import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { TranslationService } from '../../../Core/Services/translation.service';

@Component({
  selector: 'app-reset-password',
  imports: [ReactiveFormsModule, CommonModule, RouterLink, TranslateModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent implements OnInit {
  resetForm: FormGroup;
  showNewPassword = false;
  showConfirmPassword = false;
  isLoading = false;
  showSuccess = false;
  currentLanguage: string = 'en';
  showLang: boolean = false;
  private readonly authService = inject(AuthService);
  private readonly toastr = inject(ToastrService);
  private readonly activatedRouteService = inject(ActivatedRoute);
  private readonly routerService = inject(Router);
  private readonly translation = inject(TranslateService);
  private readonly _translate = inject(TranslationService);

  constructor(private fb: FormBuilder) {
    this.resetForm = this.fb.group({
      email: ['', [Validators.required]],
      token: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.pattern(/[^a-zA-Z_0-9 ]+/)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.activatedRouteService.paramMap.subscribe((paramList) => {
      this.resetForm.get("email")?.setValue(paramList.get("email"));
      this.resetForm.get("token")?.setValue(paramList.get("token"));
    });

    if (localStorage.getItem('lang') == 'ar') {
      this.currentLanguage = 'ar';
    }

  }

  passwordMatchValidator(form: FormGroup) {
    const newPassword = form.get('newPassword');
    const confirmPassword = form.get('confirmPassword');

    if (newPassword?.value !== confirmPassword?.value) {
      return { passwordMismatch: true };
    }
    return null;
  }

  toggleNewPassword() {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPassword() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  calculatePasswordStrength(): number {
    const password = this.resetForm.get('newPassword')?.value || '';
    let strength = 0;

    if (password.length >= 6) strength++;
    if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength++;
    if (/\d/.test(password)) strength++;
    if (/[^a-zA-Z0-9]/.test(password)) strength++;

    return strength;
  }

  getStrengthBarClass(index: number): string {
    const strength = this.calculatePasswordStrength();
    const baseClass = 'h-2 flex-1 rounded-full transition-colors duration-300';

    if (index < strength) {
      if (strength === 1) return `${baseClass} bg-red-500`;
      if (strength === 2) return `${baseClass} bg-yellow-500`;
      if (strength === 3) return `${baseClass} bg-blue-500`;
      if (strength === 4) return `${baseClass} bg-green-500`;
    }

    return `${baseClass} bg-gray-600`;
  }

  getPasswordStrengthText(): string {
    const strength = this.calculatePasswordStrength();
    const texts = ['Very Weak', 'Weak', 'Fair', 'Good', 'Strong'];
    return texts[strength] || 'Very Weak';
  }

  onSubmit() {
    if (this.resetForm.valid) {
      console.log(this.resetForm.value);
      this.isLoading = true;
      this.showSuccess = false;

      this.authService.resetPassword(this.resetForm.value).subscribe({
        next: (res: any) => {
          console.log(res);
          this.showSuccess = true;
          this.toastr.success(res.message, this.translation.instant("password.Reset Email is sent"));
          this.isLoading = false;
          this.routerService.navigate(['/login']);
        },
        error: (err) => {
          console.log(err.error);
          this.toastr.error(err.error.error, this.translation.instant("password.Error !!"));
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
