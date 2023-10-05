import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-view-details-modal',
  templateUrl: './view-details-modal.component.html',
  styleUrls: ['./view-details-modal.component.scss']
})
class ViewDetailsModalComponent implements OnInit {
  admissionName = environment.admissionName;
  @Output() successEvent = new EventEmitter();
  scholarship: ScholarshipUpdateDTO;

  updateForm: FormGroup;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor(
    public ViewDetailsModalRef: MdbModalRef<ViewDetailsModalComponent>
  ) {}

  ngOnInit(): void {
  }

}

export default ViewDetailsModalComponent;