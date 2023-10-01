import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditScholarshipPageComponent } from './edit-scholarship-page.component';

describe('EditScholarshipPageComponent', () => {
  let component: EditScholarshipPageComponent;
  let fixture: ComponentFixture<EditScholarshipPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditScholarshipPageComponent]
    });
    fixture = TestBed.createComponent(EditScholarshipPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
