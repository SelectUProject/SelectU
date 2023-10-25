import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllReviewsModalComponent } from './all-reviews-modal.component';

describe('AllReviewsModalComponent', () => {
  let component: AllReviewsModalComponent;
  let fixture: ComponentFixture<AllReviewsModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllReviewsModalComponent]
    });
    fixture = TestBed.createComponent(AllReviewsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
