import { Component, EventEmitter, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
import { ScholarshipApplicationService } from 'src/app/providers/ScholarshipApplicationService';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-view-application-detail-modal',
  templateUrl: './view-application-detail-modal.component.html',
  styleUrls: ['./view-application-detail-modal.component.scss'],
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
    public ViewDetailsModalRef: MdbModalRef<ViewApplicationDetailModalComponent>,
    private scholarshipApplicationService: ScholarshipApplicationService
  ) {}

  ngOnInit(): void {}

  fileDownload(fileUri: string) {
    this.scholarshipApplicationService
      .fileDownload(fileUri)
      .then((response) => {})
      .catch((response) => {});
  }
}

export default ViewApplicationDetailModalComponent;
