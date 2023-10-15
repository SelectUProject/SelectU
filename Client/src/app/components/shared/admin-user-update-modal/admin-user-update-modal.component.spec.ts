import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserUpdateModalComponent } from './admin-user-update-modal.component';

describe('AdminUserUpdateModalComponent', () => {
  let component: AdminUserUpdateModalComponent;
  let fixture: ComponentFixture<AdminUserUpdateModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminUserUpdateModalComponent]
    });
    fixture = TestBed.createComponent(AdminUserUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
