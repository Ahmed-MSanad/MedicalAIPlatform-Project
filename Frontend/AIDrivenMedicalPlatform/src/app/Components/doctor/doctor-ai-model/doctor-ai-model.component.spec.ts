import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorAiModelComponent } from './doctor-ai-model.component';

describe('DoctorAiModelComponent', () => {
  let component: DoctorAiModelComponent;
  let fixture: ComponentFixture<DoctorAiModelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorAiModelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorAiModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
