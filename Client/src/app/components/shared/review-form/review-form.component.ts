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
    return this.reviewForm.get('comments');
  }

  constructor(
    private formBuilder: FormBuilder,
    private reviewService: ReviewService
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  setupForm() {
    this.reviewForm = this.formBuilder.group({
      rating: [5, Validators.required],
      comments: [''],
    });
  }

  async review() {
    this.reviewing = true;
    this.isError = false;

    let reviewForm = <ReviewDTO>this.reviewForm.value;
    reviewForm.ScholarshipApplicationId = this.scholarshipApplicationId;

    await this.reviewService
      .review(reviewForm)
      .then(() => {
        this.success = true;
        this.successEvent.emit('Review added successfully!');
        console.log('Review Form');
      })
      .catch((response) => {
        this.errMsg = response.error.message;
        this.isError = true;
      })
      .finally(() => {
        this.reviewing = false;
      });
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.reviewForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default ReviewFormComponent;
