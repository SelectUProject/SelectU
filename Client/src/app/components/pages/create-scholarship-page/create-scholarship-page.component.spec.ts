import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateScholarshipPageComponent } from './create-scholarship-page.component';

describe('CreateScholarshipPageComponent', () => {
  let component: CreateScholarshipPageComponent;
  let fixture: ComponentFixture<CreateScholarshipPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateScholarshipPageComponent]
    });
    fixture = TestBed.createComponent(CreateScholarshipPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
