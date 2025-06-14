import { Component, inject } from '@angular/core';
import { UserService } from '../../../Core/Services/user.service';
import { UserProfile } from '../../../Core/Interfaces/user-profile';
import { GenderPipe } from '../../../Core/Pipes/gender/gender.pipe';
import { Router } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { FormArray, FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../../../Core/Services/ForAdmin/admin.service';
import { BackgroundLayoutComponent } from "../../../Layouts/background-layout/background-layout.component";
@Component({
  selector: 'app-admin-profile',
  imports: [GenderPipe, ReactiveFormsModule, BackgroundLayoutComponent],
  templateUrl: './admin-profile.component.html',
  styleUrl: './admin-profile.component.scss'
})
export class AdminProfileComponent {
  
  private readonly _user = inject(UserService);
  private readonly _admin = inject(AdminService);
  private readonly _router = inject(Router);
  private readonly _auth = inject(AuthService);
  private readonly _toastr = inject(ToastrService);

  private readonly _formBuilder = inject(FormBuilder);
  profileForm = this._formBuilder.group({
    email: [''],
    fullName: ['', [Validators.required]],
    adminPhones: this._formBuilder.array([this._formBuilder.control('', [
      Validators.required,
      Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
    ])]),
    dateOfBirth: [new Date(), [Validators.required]],
    gender: [0, [Validators.required]],
    address: ['', [Validators.required]],
    specialisation: ['',[Validators.required]],
    identificationNumber: ['',[Validators.required]],
    medicalLicenseNumber: ['',[Validators.required]],
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
    this._admin.getAdminProfile().subscribe({
      next: (res: any) => {
        // Patch non-phone values first
        console.log(res)
        const { adminPhones, ...rest } = res;
        this.profileForm.patchValue(rest);

        this.adminPhones.clear();
        res.adminPhones.forEach((phone: string) => {
          this.adminPhones.push(this._formBuilder.control(phone, [
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

  get adminPhones(): FormArray {
  return this.profileForm.get('adminPhones') as FormArray;
}

  getPhoneControl(index: number): FormControl {
    return this.adminPhones.at(index) as FormControl;
  }

  addPhone(): void {
    this.adminPhones.push(this._formBuilder.control('', [
      Validators.required,
      Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
    ]));
  }

  removePhone(index: number): void {
  if (this.adminPhones.length > 1) {
    this.adminPhones.removeAt(index);
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
      this._admin.updateAdminProfile(this.profileForm.value).subscribe({
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
    this.adminPhones.clear();
    (this.oldData.adminPhones as string[]).forEach((phone: string) => {
      this.adminPhones.push(this._formBuilder.control(phone, [
        Validators.required,
        Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)
      ]));
    });
    const { adminPhones, ...rest } = this.oldData;
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
