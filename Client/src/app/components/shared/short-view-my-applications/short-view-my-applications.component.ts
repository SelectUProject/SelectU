import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
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

@Component({
  selector: 'app-short-view-my-applications',
  templateUrl: './short-view-my-applications.component.html',
  styleUrls: ['./short-view-my-applications.component.scss'],
})
export class ShortViewMyApplicationsComponent {
  @Input() scholarshipApplication: ScholarshipApplicationUpdateDTO;
  @Input() statusText: string;
  @Input() statusIcon: string;
  success = false;
  viewDetailsModalRef: MdbModalRef<ViewApplicationDetailModalComponent>;
  reviewModalRef: MdbModalRef<ReviewModalComponent>;
  applicationStatus = ApplicationStatusEnum;
  applicationStatuses = APPLICATION_STATUS_LIST;
  USER = USER;

  constructor(
    public tokenService: TokenService,
    public scholarshipService: ScholarshipService,
    private modalService: MdbModalService
  ) {}

  openReviewModal(scholarshipApplication: ScholarshipApplicationUpdateDTO) {
    this.success = false;
    this.reviewModalRef = this.modalService.open(ReviewModalComponent, {
      data: { scholarshipApplication },
    });
    this.reviewModalRef.component.successEvent.subscribe(() => {
      this.success = true;
      this.reviewModalRef.close();
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
