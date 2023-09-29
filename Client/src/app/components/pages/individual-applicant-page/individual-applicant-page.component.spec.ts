import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndividualApplicantPageComponent } from './individual-applicant-page.component';

describe('IndividualApplicantPageComponent', () => {
  let component: IndividualApplicantPageComponent;
  let fixture: ComponentFixture<IndividualApplicantPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndividualApplicantPageComponent]
    });
    fixture = TestBed.createComponent(IndividualApplicantPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
