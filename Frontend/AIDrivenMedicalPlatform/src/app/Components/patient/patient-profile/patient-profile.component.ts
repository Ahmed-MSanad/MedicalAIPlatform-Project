import { Component, inject } from '@angular/core';
import { UserService } from '../../../Core/Services/user.service';
import { UserProfile } from '../../../Core/Interfaces/user-profile';
import { GenderPipe } from '../../../Core/Pipes/gender/gender.pipe';
import { Router } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { FormArray, FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PatientService } from '../../../Core/Services/ForPatient/patient.service';

@Component({
  selector: 'app-patient-profile',
  imports: [GenderPipe, ReactiveFormsModule],
  templateUrl: './patient-profile.component.html',
  styleUrl: './patient-profile.component.scss'
})
export class PatientProfileComponent {
  private readonly _user = inject(UserService);
  private readonly _patient = inject(PatientService);
  private readonly _router = inject(Router);
  private readonly _auth = inject(AuthService);
  private readonly _toastr = inject(ToastrService);

  private readonly _formBuilder = inject(FormBuilder);
  profileForm = this._formBuilder.group({
    email: [''],
    fullName: ['', [Validators.required]],
    patientPhones: this._formBuilder.array([this._formBuilder.control('', [
      Validators.required,
      Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
    ])]),
    dateOfBirth: [new Date(), [Validators.required]],
    gender: [0, [Validators.required]],
    address: ['', [Validators.required]],
    occupation: ['', [Validators.required]],
    emergencyContactName: ['', Validators.required],
    emergencyContactNumber: ['', [
      Validators.required,
      Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
    ]],
    familyMedicalHistory: [''],
    pastMedicalHistory: [''],
    image: [null as string | ArrayBuffer | null]
  });

  userProfile: UserProfile = {} as UserProfile;
  isEdit: boolean = false;
  isDelete: boolean = false;
  oldData: any;
  isLoading: boolean = false;
  imageSrc: string = '';

  ngOnInit(): void {
    this.isLoading = true;
    this._patient.getPatientProfile().subscribe({
      next: (res: any) => {
        // Patch non-phone values first
        const { patientPhones, ...rest } = res;
        this.profileForm.patchValue(rest);

        this.patientPhones.clear();
        res.patientPhones.forEach((phone: string) => {
          this.patientPhones.push(this._formBuilder.control(phone, [
            Validators.required,
            Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
          ]));
        });

        if (res.image) {
          this.imageSrc = `data:image/jpeg;base64,${res.image}`;
        }
        this.isLoading = false;
        this.oldData = this.profileForm.value;
      },
      error: (err: any) => {
        this._toastr.error('Failed to load profile');
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  get patientPhones(): FormArray {
  return this.profileForm.get('patientPhones') as FormArray;
}

  getPhoneControl(index: number): FormControl {
    return this.patientPhones.at(index) as FormControl;
  }

  addPhone(): void {
    this.patientPhones.push(this._formBuilder.control('', [
      Validators.required,
      Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
    ]));
  }

  removePhone(index: number): void {
  if (this.patientPhones.length > 1) {
    this.patientPhones.removeAt(index);
  }
}

  ToggleEdit() {
    this.isEdit = !this.isEdit;
    if (this.isEdit) {
      this.oldData = { ...this.profileForm.value };
    }
  }

  ToggleDelete() {
    this.isDelete = !this.isDelete;
  }

  DeleteUser() {
    this._user.deleteUserProfile().subscribe({
      next: () => {
        this._toastr.success("User deleted successfully");
        this._auth.deleteToken();
        this._router.navigateByUrl('/login');
      },
      error: (err: any) => {
        this._toastr.error("Failed to delete user");
        console.error(err);
      }
    });
  }

  SaveChanges() {
    if (this.profileForm.valid) {
      this.isLoading = true;
      this._patient.updatePatientProfile(this.profileForm.value).subscribe({
        next: () => {
          this._toastr.success("Profile updated successfully");
          this.isEdit = false;
          this.isLoading = false;
        },
        error: (err: any) => {
          this._toastr.error("Failed to update profile");
          this.CancelChanges();
          console.error(err);
          this.isLoading = false;
        }
      });
    } else {
      this._toastr.warning("Please fill all required fields correctly");
    }
  }

  CancelChanges() {
    this.isEdit = false;
    this.patientPhones.clear();
    (this.oldData.patientPhones as string[]).forEach((phone: string) => {
      this.patientPhones.push(this._formBuilder.control(phone, [
        Validators.required,
        Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
      ]));
    });
    const { patientPhones, ...rest } = this.oldData;
    this.profileForm.patchValue(rest);
  }

  ChangeImage(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.[0]) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imageSrc = reader.result as string;
        const base64 = (reader.result as string).split(',')[1];
        this.profileForm.get('image')?.setValue(base64);
      };
      reader.readAsDataURL(input.files[0]);
    }
  }

  validatePhone(control: FormControl, index: number): void {
  if (control.invalid) {
    control.markAsTouched();
  }
}
}