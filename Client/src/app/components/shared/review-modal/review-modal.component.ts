import { Component, EventEmitter, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import ViewApplicationDetailModalComponent from '../view-application-detail-modal/view-application-detail-modal.component';
import { ReviewDTO } from 'src/app/models/ReviewDTO';

@Component({
  selector: 'app-review-modal',
  templateUrl: './review-modal.component.html',
  styleUrls: ['./review-modal.component.scss'],
})
class ReviewModalComponent {
  @Output() successEvent = new EventEmitter<string>();
  scholarshipApplication: ScholarshipApplicationUpdateDTO;
  reviewDTO: ReviewDTO;

  reviewForm: FormGroup;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor(
    public ViewDetailsModalRef: MdbModalRef<ViewApplicationDetailModalComponent>
  ) {}

  ngOnInit(): void {
    console.log(this.reviewDTO);
  }

  handleSuccess(message: string) {
    this.successEvent.emit(message);
  }
}

export default ReviewModalComponent;
