import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScholarshipApplicationFormComponent } from './scholarship-application-form.component';

describe('ScholarshipApplicationFormComponent', () => {
  let component: ScholarshipApplicationFormComponent;
  let fixture: ComponentFixture<ScholarshipApplicationFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScholarshipApplicationFormComponent]
    });
    fixture = TestBed.createComponent(ScholarshipApplicationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
