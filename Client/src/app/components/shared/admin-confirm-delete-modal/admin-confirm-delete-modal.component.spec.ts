import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminConfirmDeleteModalComponent } from './admin-confirm-delete-modal.component';

describe('AdminConfirmDeleteModalComponent', () => {
  let component: AdminConfirmDeleteModalComponent;
  let fixture: ComponentFixture<AdminConfirmDeleteModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminConfirmDeleteModalComponent]
    });
    fixture = TestBed.createComponent(AdminConfirmDeleteModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
