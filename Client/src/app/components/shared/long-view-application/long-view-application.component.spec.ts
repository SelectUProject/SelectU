import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LongViewApplicationComponent } from './long-view-application.component';

describe('LongViewApplicationComponent', () => {
  let component: LongViewApplicationComponent;
  let fixture: ComponentFixture<LongViewApplicationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LongViewApplicationComponent]
    });
    fixture = TestBed.createComponent(LongViewApplicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
