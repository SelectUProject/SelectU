import { Component } from '@angular/core';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { ReviewDTO } from 'src/app/models/ReviewDTO';

@Component({
  selector: 'app-all-reviews-modal',
  templateUrl: './all-reviews-modal.component.html',
  styleUrls: ['./all-reviews-modal.component.scss'],
})
export class AllReviewsModalComponent {
  reviews: ReviewDTO[];

  constructor(
    public ViewDetailsModalRef: MdbModalRef<AllReviewsModalComponent>
  ) {}
}
