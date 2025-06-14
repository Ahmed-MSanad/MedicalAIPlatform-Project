import { CommonModule } from '@angular/common';
import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms'
import { AuthService } from '../../../Core/Services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { Router, RouterLink } from '@angular/router';


@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  animations:[
    trigger('shakeAnimation', [
      transition('false => true', [
        animate('0.5s', keyframes([
          style({ transform: 'rotate(15deg)', offset: 0.2 }),
          style({ transform: 'rotate(-15deg)', offset: 0.4 }),
          style({ transform: 'rotate(15deg)', offset: 0.6 }),
          style({ transform: 'rotate(-15deg)', offset: 0.8 }),
          style({ transform: 'rotate(0deg)', offset: 1.0 })
        ]))
      ])
    ])
  ]
})
export class RegisterComponent {

  isHovered = false

  private readonly _formBuilder = inject(FormBuilder);

  registerForm = this._formBuilder.group({
    fullName: ['',[Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, 
                    Validators.minLength(6), 
                    Validators.pattern(/[^a-zA-Z_0-9 ]+/)]], // at least 1 special character // /^$/ => ^$ are so missing 😑😑 another way --> /(?=.*[^a-zA-Z0-9 ])/
    confirmPassword: [''],
    phoneNumber: ['', [Validators.required,
                        Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)]],
    dateOfBirth: ['', [Validators.required]],
    gender: [0, [Validators.required]],
    address: ['', [Validators.required]],
    occupation: ['', [Validators.required]],
    emergencyContactName: ['', Validators.required],
    emergencyContactNumber: ['', [Validators.required,
                                  Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)]],
    familyMedicalHistory: [''],
    pastMedicalHistory: [''],
    identificationNumber: ['', [Validators.required]],
    medicalLicenseNumber: ['', [Validators.required]],
    specialisation: ['', [Validators.required]],
    workPlace: ['', [Validators.required]],
    fee:[null,[Validators.required, Validators.min(0)]], 
    role: ['', [Validators.required, Validators.pattern(/(Doctor|Admin|Patient)/)] ]
  }, {validators: [this.confirmThePassword]});


  confirmThePassword(control:AbstractControl){
    if(control.get("password")?.value === control.get("confirmPassword")?.value){
      return null;
    }
    else{
      return {passwordMismatch:true};
    }
  }


  
  private readonly _authService$ = inject(AuthService);
  private readonly _toastr = inject(ToastrService);
  private readonly _route = inject(Router);
  onSubmitRegistration(){
    if(
      (this.registerForm.controls.fullName.valid && this.registerForm.controls.email.valid && this.registerForm.controls.password.valid && this.registerForm.controls.confirmPassword.valid && this.registerForm.controls.gender.valid && this.registerForm.controls.address.valid && this.registerForm.controls.dateOfBirth.valid && this.registerForm.controls.phoneNumber.valid && this.registerForm.controls.role.valid) &&
      (this.registerForm.controls.role.value === 'Patient' && this.registerForm.controls.occupation.valid && this.registerForm.controls.emergencyContactName.valid && this.registerForm.controls.emergencyContactNumber.valid && this.registerForm.controls.familyMedicalHistory.valid && this.registerForm.controls.pastMedicalHistory.valid) ||
      (this.registerForm.controls.role.value === 'Doctor' && this.registerForm.controls.identificationNumber.valid && this.registerForm.controls.medicalLicenseNumber.valid && this.registerForm.controls.specialisation.valid && this.registerForm.controls.workPlace.valid) ||
      (this.registerForm.controls.role.value === 'Admin' && this.registerForm.controls.identificationNumber.valid && this.registerForm.controls.medicalLicenseNumber.valid && this.registerForm.controls.specialisation.valid)
    ){
      console.log(this.registerForm.value);

      this._authService$.createUser(this.registerForm.value).subscribe({
        next:(res:any) =>{
          this.registerForm.reset();
          this._toastr.success("New User is created", "Registration Successful");
          this._route.navigateByUrl('login');
          console.log('response: ',res);
        },
        error:err =>{
          if(err.error.errors){
            err.error.errors.forEach((x:any) => {
              switch(x.code){
                case "DuplicateEmail":
                  this._toastr.error(x.description, "Registration Failed");
                  break;
                case "DuplicateUserName":
                  this._toastr.error(x.description, "Registration Failed");
                  break;
                default:
                  this._toastr.error("Contact the developer", "Registration Failed");
                  break;
              }
            });
          }
          else{
            console.log(err);
          }
        }
      });
    }
    else{
      // console.log(this.registerForm.errors);
      
      // console.log("Full Name valid:", this.registerForm.controls.fullName.valid);
      // console.log("Email valid:", this.registerForm.controls.email.valid);
      // console.log("Password valid:", this.registerForm.controls.password.valid);
      // console.log("Confirm Password valid:", this.registerForm.controls.confirmPassword.valid);
      // console.log("Gender valid:", this.registerForm.controls.gender.valid);
      // console.log("Address valid:", this.registerForm.controls.address.valid);
      // console.log("Date of Birth valid:", this.registerForm.controls.dateOfBirth.valid);
      // console.log("Phone Number valid:", this.registerForm.controls.phoneNumber.valid);
      // console.log("Role valid:", this.registerForm.controls.role.valid);

      // console.log("Role is 'Doctor':", this.registerForm.controls.role.value === 'Doctor');
      // console.log("Identification Number valid (Doctor):", this.registerForm.controls.identificationNumber.valid);
      // console.log("Medical License Number valid (Doctor):", this.registerForm.controls.medicalLicenseNumber.valid);
      // console.log("Specialisation valid (Doctor):", this.registerForm.controls.specialisation.valid);
      // console.log("Work Place valid (Doctor):", this.registerForm.controls.workPlace.valid);

      this.registerForm.markAllAsTouched();
    }
  }


  afterGetStarted : boolean = false;
  @ViewChild('box') box!: ElementRef;

  checkEmailSubscription ! : Subscription;
  onGetStarted() {
    const isValid = this.registerForm.controls.fullName.valid &&
      this.registerForm.controls.email.valid &&
      this.registerForm.controls.phoneNumber.valid;

    if(isValid){
      this.checkEmailSubscription = this._authService$.checkEmail({email: this.registerForm.controls.email.value}).subscribe({
        next: (res : any) =>{
          console.log(res);

          if(res?.isRegistered){
            Swal.fire({
              title: "This Email is Already registered before",
              html: `
                <form>
                    <label for="swal-password">Password:</label>
                    <input id="swal-password" type="password" class="border-2 p-4 rounded-lg" />
                </form>
              `,
              showCancelButton: true,
              confirmButtonText: "Go ahead 😃",
              cancelButtonText: "Cancel",
              focusConfirm: false,
              preConfirm:() => {
                const password = (document.getElementById("swal-password") as HTMLInputElement)?.value;
        
                if(!password){
                  Swal.showValidationMessage("Password is required to continue !");
                  setTimeout(() => {
                    Swal.resetValidationMessage();
                  }, 2000);
                  return false;
                }
        
                return {password};
              }
            }).then((result) =>{
              let loginInErrorMessage = "";
              if(result.isConfirmed){
                let loginForm = this._formBuilder.group({
                  email: [this.registerForm.controls.email.value],
                  password: [result.value.password],
                });
                this._authService$.signIn(loginForm.value).subscribe({
                  next:(res : any) => {
                    console.log(res);
                    this._authService$.saveToken(res.token);
                    const userClaims = this._authService$.getClaims();
                    this._route.navigate(['/'+userClaims.role+"Dashboard"]);
                  },
                  error: (error) => {
                    if(error.status == 400){
                      this._toastr.error(error.error.message, 'Login Failed');
                      loginInErrorMessage = error.error.message;
                      console.log(error.error.message);
                    }
                    else{
                      this._toastr.error('Error during login !!', 'Login Failed');
                      loginInErrorMessage = 'Error during login !!';
                      console.log('Error during login !!');
                    }

                    if(!loginInErrorMessage)
                      Swal.fire('Success!', 'Form submitted successfully', 'success');
                    else
                      Swal.fire('Error!', loginInErrorMessage, 'error');
                  }
                });
              }
            });
          }
          else{
            this.toggleGetStarted();
          }

        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {
          this.checkEmailSubscription?.unsubscribe();
        }
      });
    }
    else{
      this.registerForm.controls.fullName.markAsTouched();
      this.registerForm.controls.email.markAsTouched();
      this.registerForm.controls.phoneNumber.markAsTouched();
    }
  }

  toggleGetStarted(){
    const boxEl = this.box.nativeElement;
    
    if(this.afterGetStarted){
      boxEl.classList.remove('hidden');
    }
    else{
      boxEl.addEventListener('transitionend', () => {
        boxEl.classList.add('hidden');
      }, { once: true });
    }

    this.afterGetStarted = !this.afterGetStarted;
  }



  showPasswordOffOn : {[key: string] : boolean} = {};
  ngOnInit(){
    this.showPasswordOffOn["Password"] = false;
    this.showPasswordOffOn["ConfirmPassword"] = false;
  }
  togglePassword(state : string){
    this.showPasswordOffOn[state] = !this.showPasswordOffOn[state];
  }
  // ----------------------------------------------------------------------

  
  



}
