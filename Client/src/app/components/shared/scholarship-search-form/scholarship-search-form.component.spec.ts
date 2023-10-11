import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScholarshipSearchFormComponent } from './scholarship-search-form.component';

describe('ScholarshipSearchFormComponent', () => {
  let component: ScholarshipSearchFormComponent;
  let fixture: ComponentFixture<ScholarshipSearchFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScholarshipSearchFormComponent]
    });
    fixture = TestBed.createComponent(ScholarshipSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
