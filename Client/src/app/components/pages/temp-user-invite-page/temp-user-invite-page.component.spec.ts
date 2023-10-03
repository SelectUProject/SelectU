import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TempUserInvitePageComponent } from './temp-user-invite-page.component';

describe('TempUserInvitePageComponent', () => {
  let component: TempUserInvitePageComponent;
  let fixture: ComponentFixture<TempUserInvitePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TempUserInvitePageComponent]
    });
    fixture = TestBed.createComponent(TempUserInvitePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
