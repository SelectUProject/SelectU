import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCreatorAreaComponent } from '../form-builder-ui/form-builder-ui.component';

describe('FormCreatorAreaComponent', () => {
  let component: FormCreatorAreaComponent;
  let fixture: ComponentFixture<FormCreatorAreaComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FormCreatorAreaComponent]
    });
    fixture = TestBed.createComponent(FormCreatorAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
