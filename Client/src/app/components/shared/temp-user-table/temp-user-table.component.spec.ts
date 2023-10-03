import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TempUserTableComponent } from './temp-user-table.component';

describe('TempUserTableComponent', () => {
  let component: TempUserTableComponent;
  let fixture: ComponentFixture<TempUserTableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TempUserTableComponent]
    });
    fixture = TestBed.createComponent(TempUserTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
