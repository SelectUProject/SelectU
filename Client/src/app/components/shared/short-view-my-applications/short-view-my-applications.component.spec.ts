import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShortViewMyApplicationsComponent } from './short-view-my-applications.component';

describe('ShortViewMyApplicationsComponent', () => {
  let component: ShortViewMyApplicationsComponent;
  let fixture: ComponentFixture<ShortViewMyApplicationsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShortViewMyApplicationsComponent]
    });
    fixture = TestBed.createComponent(ShortViewMyApplicationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
