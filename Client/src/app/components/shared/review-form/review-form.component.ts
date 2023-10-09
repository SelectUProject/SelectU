import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReviewService } from 'src/app/providers/review.service';

@Component({
  selector: 'app-review-form',
  templateUrl: './review-form.component.html',
  styleUrls: ['./review-form.component.scss'],
})
class ReviewFormComponent {
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
      rating: ['', Validators.required],
      comments: ['', Validators.required],
    });
  }

  async review() {
    this.reviewing = true;
    this.isError = false;

    let inviteForm = <UserInviteDTO>this.reviewForm.value;
    inviteForm.loginExpiry = new Date(inviteForm.loginExpiry);

    await this.userService
      .inviteUser(inviteForm)
      .then(() => {
        this.success = true;
        this.router.navigate(['/view-users']);
      })
      .catch((response) => {
        if (response.error?.errors) {
          this.errMsg = 'One or more validation errors occurred.';
          response.error?.errors?.forEach((form: any) => {
            this.setFormError(form.propertyName);
          });
        } else if (!response.success) {
          this.errMsg = response.error.message;
        }
        this.isError = true;
      });
    this.inviting = false;
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.inviteForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default ReviewFormComponent;
