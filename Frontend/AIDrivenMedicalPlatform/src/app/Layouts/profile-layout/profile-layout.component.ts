import { Component, inject } from '@angular/core';
import { UserService } from '../../Core/Services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../../Core/Services/auth.service';
import { GenderPipe } from "../../Core/Pipes/gender/gender.pipe";
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile-layout',
  imports: [GenderPipe,ReactiveFormsModule],
  templateUrl: './profile-layout.component.html',
  styleUrl: './profile-layout.component.scss'
})
export class ProfileLayoutComponent {

  private readonly _formBuilder = inject(FormBuilder);
  profileForm = this._formBuilder.group({
      email: [''],
      fullName: ['',[Validators.required]],
      phone: ['', [Validators.required,
                          Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)]],
      dateOfBirth: [new Date(), [Validators.required]],
      gender: [0, [Validators.required]],
      address: ['', [Validators.required]],
      occupation: ['', [Validators.required]],
      emergencyContactName: ['', Validators.required],
      emergencyContactNumber: ['', [Validators.required,
                                    Validators.pattern(/^(011|010|015|012)[0-9]{8}$/)]],
      familyMedicalHistory: [''],
      pastMedicalHistory: [''],
      image: [null as string| ArrayBuffer | null]
    });

    private readonly _router = inject(Router);
    private readonly _auth = inject(AuthService);
    private readonly _user = inject(UserService);
    private readonly _toastr = inject(ToastrService);
  

    isEdit: boolean = false;
    isDelete: boolean = false;
    oldData = {...this.profileForm.value};
    isLoading: boolean = false;
    imageSrc: string = '';

  ngOnInit(): void {
    this.isLoading = true;
    this._user.getUserProfile().subscribe({
      next: (res : any) => {
        console.log(res)
        this.profileForm.patchValue(res);
        if (res.image) {
          this.imageSrc = `data:image/jpeg;base64,${res.image}`;
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.log(err);
      }
    });
    
  }

  ToggleEdit() {
    this.isEdit = !this.isEdit;
    this.oldData = { ...this.profileForm.value };
  }

  ToggleDelete() {
    this.isDelete = !this.isDelete;
  }
  DeleteUser(){
    this._user.deleteUserProfile().subscribe({
      next: (res : any) => {
        this._toastr.success("User Has Been Deleted Successfully", "User Deleted");
        this._auth.deleteToken();
        this._router.navigateByUrl('/login');
      },
      error: (err) => {
        this._toastr.error("Failed To Delete");
        console.log(err);
      }
    });
  }
  SaveChanges() {
    console.log(this.profileForm.value)
    if(this.profileForm.valid){
    this.isEdit = false;
    this._user.updateUserProfile(this.profileForm.value).subscribe({
      next: (res : any) => {
        this._toastr.success("User Profile Has Been Changed Successfully", "User Edited");
      },
      error: (err) => {
        this._toastr.error("Failed To Edit");
        this.profileForm.patchValue(this.oldData);
        console.log(err);
      }
    })
    }
  }

  CancelChanges() {
    this.isEdit = false;
    this.profileForm.patchValue(this.oldData);
  }

  ChangeImage(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.[0]) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imageSrc = reader.result as string;
        const base64 = (reader.result as string).split(',')[1]; // Remove "data:image/..."
        this.profileForm.get('image')?.setValue(base64); // Store Base64
      };
      reader.readAsDataURL(input.files[0]);
    }
  }
}
