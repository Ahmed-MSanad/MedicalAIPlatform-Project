import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientDoctorResponseComponent } from './patient-doctor-response.component';

describe('PatientDoctorResponseComponent', () => {
  let component: PatientDoctorResponseComponent;
  let fixture: ComponentFixture<PatientDoctorResponseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientDoctorResponseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PatientDoctorResponseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
