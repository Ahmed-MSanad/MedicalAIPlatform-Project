import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackgroundLayoutComponent } from './background-layout.component';

describe('BackgroundLayoutComponent', () => {
  let component: BackgroundLayoutComponent;
  let fixture: ComponentFixture<BackgroundLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BackgroundLayoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BackgroundLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
