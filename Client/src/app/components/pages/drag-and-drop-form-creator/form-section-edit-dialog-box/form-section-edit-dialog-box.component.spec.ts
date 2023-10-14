import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSectionEditDialogBoxComponent } from './form-section-edit-dialog-box.component';

describe('FormSectionEditDialogBoxComponent', () => {
  let component: FormSectionEditDialogBoxComponent;
  let fixture: ComponentFixture<FormSectionEditDialogBoxComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FormSectionEditDialogBoxComponent]
    });
    fixture = TestBed.createComponent(FormSectionEditDialogBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
