import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TempUserTablePageComponent } from './temp-user-table-page.component';

describe('TempUserTablePageComponent', () => {
  let component: TempUserTablePageComponent;
  let fixture: ComponentFixture<TempUserTablePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TempUserTablePageComponent]
    });
    fixture = TestBed.createComponent(TempUserTablePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
