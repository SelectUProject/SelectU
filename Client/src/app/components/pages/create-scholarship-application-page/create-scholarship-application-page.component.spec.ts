import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateScholarshipApplicationPageComponent } from './create-scholarship-application-page.component';

describe('CreateScholarshipApplicationPageComponent', () => {
  let component: CreateScholarshipApplicationPageComponent;
  let fixture: ComponentFixture<CreateScholarshipApplicationPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateScholarshipApplicationPageComponent]
    });
    fixture = TestBed.createComponent(CreateScholarshipApplicationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
