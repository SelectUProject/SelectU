import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TempUserInviteFormComponent } from './temp-user-invite-form.component';

describe('TempUserInviteFormComponent', () => {
  let component: TempUserInviteFormComponent;
  let fixture: ComponentFixture<TempUserInviteFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TempUserInviteFormComponent]
    });
    fixture = TestBed.createComponent(TempUserInviteFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
