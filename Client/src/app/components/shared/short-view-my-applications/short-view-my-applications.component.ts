import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { MyApplicationShortViewDTO } from '../../../models/MyApplicationShortViewDTO';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ApplicationStatusEnum } from 'src/app/models/ApplicationStatusEnum';
import ViewApplicationDetailModalComponent from '../view-application-detail-modal/view-application-detail-modal.component';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { TokenService } from 'src/app/providers/token.service';
import ViewDetailsModalComponent from '../view-details-modal/view-details-modal.component';
import { APPLICATION_STATUS_LIST } from 'src/app/constants/ApplicationStatus';
import ReviewModalComponent from '../review-modal/review-modal.component';
import { USER } from 'src/app/constants/userRoles';
import { ReviewService } from 'src/app/providers/review.service';
import { ReviewDTO } from 'src/app/models/ReviewDTO';
import { ScholarshipApplicationService } from 'src/app/providers/application.service';
import { AllReviewsModalComponent } from '../all-reviews-modal/all-reviews-modal.component';

@Component({
  selector: 'app-short-view-my-applications',
  templateUrl: './short-view-my-applications.component.html',
  styleUrls: ['./short-view-my-applications.component.scss'],
})
export class ShortViewMyApplicationsComponent implements OnInit {
  @Input() scholarshipApplication: ScholarshipApplicationUpdateDTO;
  @Input() statusText: string;
  @Input() statusIcon: string;
  rating?: number;
  review?: ReviewDTO;
  success = false;
  successMessage: string;
  viewDetailsModalRef: MdbModalRef<ViewApplicationDetailModalComponent>;
  reviewModalRef: MdbModalRef<ReviewModalComponent>;
  allReviewModalRef: MdbModalRef<AllReviewsModalComponent>;
  applicationStatus = ApplicationStatusEnum;
  applicationStatuses = APPLICATION_STATUS_LIST;
  USER = USER;

  constructor(
    public tokenService: TokenService,
    public scholarshipService: ScholarshipService,
    public scholarshipApplicationService: ScholarshipApplicationService,
    private modalService: MdbModalService,
    private reviewService: ReviewService
  ) {}

  ngOnInit(): void {
    if (this.tokenService.role !== USER) {
      this.getAverageRating();
      this.getMyReview();
    }
  }

  getAverageRating() {
    this.reviewService
      .getAverageRating(this.scholarshipApplication.id)
      .then((response) => {
        this.rating = response;
      })
      .catch((response) => {
        console.log(response.error.message);
      });
  }

  getMyReview() {
    this.reviewService
      .getMyReview(this.scholarshipApplication.id)
      .then((response) => {
        this.review = response;
      })
      .catch((response) => {
        console.log(response.error.message);
      });
  }

  deleteReview(reviewDTO: ReviewDTO) {
    this.reviewService
      .deleteReview(reviewDTO.id)
      .then((response) => {
        this.successMessage = 'Review deleted!';
        this.success = true;
        this.getAverageRating();
        this.getMyReview();
      })
      .catch((response) => {
        console.log(response.error.message);
      });
  }

  selectApplication(scholarshipApplication: ScholarshipApplicationUpdateDTO) {
    this.success = false;
    this.scholarshipApplicationService
      .selectApplication(scholarshipApplication)
      .then((response) => {
        this.successMessage = response.message;
        this.success = true;
      })
      .catch((response) => {
        console.log(response.error.message);
      });
  }

  openReviewModal(
    scholarshipApplication: ScholarshipApplicationUpdateDTO,
    reviewDTO?: ReviewDTO
  ) {
    this.success = false;
    this.reviewModalRef = this.modalService.open(ReviewModalComponent, {
      data: { scholarshipApplication, reviewDTO },
    });
    this.reviewModalRef.component.successEvent.subscribe((message) => {
      this.successMessage = message;
      this.success = true;
      this.reviewModalRef.close();

      this.getAverageRating();
      this.getMyReview();
    });
  }

  openAllReviewModal() {
    this.success = false;
    this.reviewService
      .getAllReviews(this.scholarshipApplication.id)
      .then((response) => {
        this.allReviewModalRef = this.modalService.open(
          AllReviewsModalComponent,
          {
            data: { reviews: response },
          }
        );
      })
      .catch((response) => {
        console.log(response.error.message);
      });
  }

  openDetailsModal(scholarshipApplication: ScholarshipApplicationUpdateDTO) {
    this.success = false;
    this.viewDetailsModalRef = this.modalService.open(
      ViewApplicationDetailModalComponent,
      {
        data: { scholarshipApplication },
      }
    );
    this.viewDetailsModalRef.component.successEvent.subscribe(() => {
      this.success = true;
      // this.getAllUsers();
      this.viewDetailsModalRef.close();
    });
  }
}
