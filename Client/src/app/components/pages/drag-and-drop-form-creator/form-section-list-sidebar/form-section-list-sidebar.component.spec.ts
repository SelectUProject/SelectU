import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSectionListSidebarComponent } from './form-section-list-sidebar.component';

describe('FormSectionListSidebarComponent', () => {
  let component: FormSectionListSidebarComponent;
  let fixture: ComponentFixture<FormSectionListSidebarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FormSectionListSidebarComponent]
    });
    fixture = TestBed.createComponent(FormSectionListSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
