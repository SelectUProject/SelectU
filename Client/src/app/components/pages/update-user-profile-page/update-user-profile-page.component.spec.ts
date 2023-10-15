import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateUserProfilePageComponent } from './update-user-profile-page.component';

describe('UpdateUserProfilePageComponent', () => {
  let component: UpdateUserProfilePageComponent;
  let fixture: ComponentFixture<UpdateUserProfilePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateUserProfilePageComponent]
    });
    fixture = TestBed.createComponent(UpdateUserProfilePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
