import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-long-view-application',
  templateUrl: './long-view-application.component.html',
  styleUrls: ['./long-view-application.component.scss'],
})
class LongViewApplicationComponent {
  admissionName = environment.admissionName;
  @Output() successEvent = new EventEmitter();
  @Input() scholarshipApplication: ScholarshipApplicationUpdateDTO;

  updateForm: FormGroup;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor() {}
}

export default LongViewApplicationComponent;
