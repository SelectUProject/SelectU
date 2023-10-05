import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { ScholarshipApplicationDTO } from 'src/app/models/ScholarshipApplicationDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { UserService } from 'src/app/providers/user.service';
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

  apply(){

  }

}

export default ViewDetailsModalComponent;