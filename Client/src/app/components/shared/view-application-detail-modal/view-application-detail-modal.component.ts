import { Component, EventEmitter, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { NumberLookupDTO } from 'src/app/models/LookupDTOs';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-view-application-detail-modal',
  templateUrl: './view-application-detail-modal.component.html',
  styleUrls: ['./view-application-detail-modal.component.scss']
})
export class ViewApplicationDetailModalComponent {
  ScholarshipFormTypeEnum = ScholarshipFormTypeEnum;

  admissionName = environment.admissionName;
  @Output() successEvent = new EventEmitter();
  scholarshipApplication: ScholarshipApplicationUpdateDTO;

  updateForm: FormGroup;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor(
    public ViewDetailsModalRef: MdbModalRef<ViewApplicationDetailModalComponent>
  ) {}

  ngOnInit(): void {
  }

}

export default ViewApplicationDetailModalComponent;