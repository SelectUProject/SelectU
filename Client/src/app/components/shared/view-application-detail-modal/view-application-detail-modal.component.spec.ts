import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewApplicationDetailModalComponent } from './view-application-detail-modal.component';

describe('ViewApplicationDetailModalComponent', () => {
  let component: ViewApplicationDetailModalComponent;
  let fixture: ComponentFixture<ViewApplicationDetailModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewApplicationDetailModalComponent]
    });
    fixture = TestBed.createComponent(ViewApplicationDetailModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
