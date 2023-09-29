import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateScholarshipComponent } from './create-scholarship.component';

describe('CreateScholarshipComponent', () => {
  let component: CreateScholarshipComponent;
  let fixture: ComponentFixture<CreateScholarshipComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateScholarshipComponent]
    });
    fixture = TestBed.createComponent(CreateScholarshipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
