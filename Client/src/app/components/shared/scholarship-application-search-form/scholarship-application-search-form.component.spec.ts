import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScholarshipApplicationSearchFormComponent } from './scholarship-application-search-form.component';

describe('ScholarshipApplicationSearchFormComponent', () => {
  let component: ScholarshipApplicationSearchFormComponent;
  let fixture: ComponentFixture<ScholarshipApplicationSearchFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScholarshipApplicationSearchFormComponent]
    });
    fixture = TestBed.createComponent(ScholarshipApplicationSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
