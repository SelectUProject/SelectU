import { ComponentFixture, TestBed } from '@angular/core/testing';

import UserTablePageComponent from './user-table-page.component';

describe('UserTablePageComponent', () => {
  let component: UserTablePageComponent;
  let fixture: ComponentFixture<UserTablePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserTablePageComponent],
    });
    fixture = TestBed.createComponent(UserTablePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
