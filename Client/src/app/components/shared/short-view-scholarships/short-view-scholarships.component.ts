import { Component, Input } from '@angular/core';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { TokenService } from 'src/app/providers/token.service';
import { ADMIN, STAFF, USER } from 'src/app/constants/userRoles';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { Router } from '@angular/router';
import ViewDetailsModalComponent from '../view-details-modal/view-details-modal.component';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';

@Component({
  selector: 'app-short-view-scholarships',
  templateUrl: './short-view-scholarships.component.html',
  styleUrls: ['./short-view-scholarships.component.scss'],
})
export class ShortViewScholarshipsComponent {
  ADMIN = ADMIN;
  STAFF = STAFF;
  USER = USER;
  success = false;
  viewDetailsModalRef:MdbModalRef<ViewDetailsModalComponent>;
  
  @Input() scholarship: ScholarshipUpdateDTO;

  set data(value: ScholarshipUpdateDTO) {
    this.scholarshipService.scholarship = value;
  }

  constructor(
    private router: Router,
    public tokenService: TokenService,
    public scholarshipService: ScholarshipService,
    private modalService: MdbModalService
  ) {}

  openModal(scholarship: ScholarshipUpdateDTO) {
    this.scholarshipService.scholarship = this.scholarship
    this.success = false;
    this.viewDetailsModalRef = this.modalService.open(ViewDetailsModalComponent, {
      data: { scholarship },
    });
    this.viewDetailsModalRef.component.successEvent.subscribe(() => {
      this.success = true;
      // this.getAllUsers();
      this.viewDetailsModalRef.close();
    });
  }
  
}
