import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TempUserUpdateModalComponent } from './temp-user-update-modal.component';

describe('TempUserUpdateModalComponent', () => {
  let component: TempUserUpdateModalComponent;
  let fixture: ComponentFixture<TempUserUpdateModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TempUserUpdateModalComponent]
    });
    fixture = TestBed.createComponent(TempUserUpdateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
