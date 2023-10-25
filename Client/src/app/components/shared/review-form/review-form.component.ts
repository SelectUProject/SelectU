import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReviewDTO } from 'src/app/models/ReviewDTO';
import { ReviewService } from 'src/app/providers/review.service';

@Component({
  selector: 'app-review-form',
  templateUrl: './review-form.component.html',
  styleUrls: ['./review-form.component.scss'],
})
class ReviewFormComponent {
  @Input() scholarshipApplicationId: string;
  @Input() reviewDTO?: ReviewDTO;
  @Output() successEvent = new EventEmitter<string>();

  reviewForm: FormGroup;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  reviewing: boolean = false;
  success: boolean = false;

  get rating() {
    return this.reviewForm.get('rating');
  }

  get comments() {
    return this.reviewForm.get('comment');
  }

  constructor(
    private formBuilder: FormBuilder,
    private reviewService: ReviewService
  ) {}

  ngOnInit(): void {
    console.log(this.reviewDTO);
    this.setupForm();
  }

  setupForm() {
    this.reviewForm = this.formBuilder.group({
      rating: [this.reviewDTO?.rating ?? 5, Validators.required],
      comment: [this.reviewDTO?.comment ?? '']
    });
  }

  async review() {
    this.reviewing = true;
    this.isError = false;

    let reviewForm = <ReviewDTO>this.reviewForm.value;
    reviewForm.scholarshipApplicationId = this.scholarshipApplicationId;
    if (this.reviewDTO) {
      reviewForm.id = this.reviewDTO?.id;
      await this.reviewService
        .updateReview(reviewForm)
        .then(() => {
          this.success = true;
          this.successEvent.emit('Review updated successfully!');
          this.reviewForm.reset();
        })
        .catch((response) => {
          this.errMsg = response.error.message;
          this.isError = true;
        })
        .finally(() => {
          this.reviewing = false;
        });
    } else {
      await this.reviewService
        .review(reviewForm)
        .then(() => {
          this.success = true;
          this.successEvent.emit('Review added successfully!');
          this.reviewForm.reset();
        })
        .catch((response) => {
          this.errMsg = response.error.message;
          this.isError = true;
        })
        .finally(() => {
          this.reviewing = false;
        });
    }
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.reviewForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default ReviewFormComponent;
