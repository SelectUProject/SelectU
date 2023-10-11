import { ComponentFixture, TestBed } from '@angular/core/testing';

import UserInviteFormComponent from './user-invite-form.component';

describe('UserInviteFormComponent', () => {
  let component: UserInviteFormComponent;
  let fixture: ComponentFixture<UserInviteFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserInviteFormComponent],
    });
    fixture = TestBed.createComponent(UserInviteFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
