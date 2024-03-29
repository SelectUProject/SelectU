import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/providers/token.service';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipApplicationService } from 'src/app/providers/application.service';
import { ScholarshipApplicationSearchDTO } from 'src/app/models/ScholarshipApplicationSearchDTO';
import { ApplicationStatusEnum } from 'src/app/models/ApplicationStatusEnum';

@Component({
  selector: 'app-scholarship-application-search-form',
  templateUrl: './scholarship-application-search-form.component.html',
  styleUrls: ['./scholarship-application-search-form.component.scss'],
})
export class ScholarshipApplicationSearchFormComponent implements OnInit {
  searchScholarshipApplicationForm: FormGroup;
  submitting: boolean = false;
  isError: boolean = false;
  errMsg: string = 'Some error has occurred!';
  todayDate: Date = new Date();

  @Output() scholarshipApplications = new EventEmitter<
    ScholarshipApplicationUpdateDTO[]
  >();

  constructor(
    private formBuilder: FormBuilder,
    private scholarshipApplicationService: ScholarshipApplicationService
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  setupForm() {
    this.searchScholarshipApplicationForm = this.formBuilder.group({
      id: [null, ''],
      school: [null, ''],
      status: [null, ''],
      dateCreated: [null, ''],
    });
  }

  search() {
    this.submitting = true;
    this.isError = false;
    this.scholarshipApplicationService
      .getMyScholarshipApplications(
        <ScholarshipApplicationSearchDTO>(
          this.searchScholarshipApplicationForm.value
        )
      )
      .then((response) => {
        this.scholarshipApplications.emit(response);
      })
      .catch((response) => {
        console.error(response);
      })
      .finally(() => {
        this.submitting = false;
      });
  }
}
