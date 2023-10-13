import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewApplicationsPageComponent } from './view-applications-page.component';

describe('ViewApplicationsPageComponent', () => {
  let component: ViewApplicationsPageComponent;
  let fixture: ComponentFixture<ViewApplicationsPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewApplicationsPageComponent]
    });
    fixture = TestBed.createComponent(ViewApplicationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
