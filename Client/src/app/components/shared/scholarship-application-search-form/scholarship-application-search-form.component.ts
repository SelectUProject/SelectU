import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/providers/token.service';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipApplicationService } from 'src/app/providers/ScholarshipApplicationService';
import { ScholarshipApplicationSearchDTO } from 'src/app/models/ScholarshipApplicationSearchDTO';
import { ApplicationStatusEnum } from 'src/app/models/ApplicationStatusEnum';

@Component({
  selector: 'app-scholarship-application-search-form',
  templateUrl: './scholarship-application-search-form.component.html',
  styleUrls: ['./scholarship-application-search-form.component.scss'],
})
export class ScholarshipApplicationSearchFormComponent implements OnInit {
  searchScholarshipForm: FormGroup;
  submitting: boolean = false;
  isError: boolean = false;
  errMsg: string = 'Some error has occurred!';
  // scholarships: ScholarshipUpdateDTO[] = [];
  todayDate: Date = new Date();

  @Output() scholarships = new EventEmitter<
    ScholarshipApplicationUpdateDTO[]
  >();

  constructor(
    private formBuilder: FormBuilder,
    private scholarshipService: ScholarshipService,
    private scholarshipApplicationService: ScholarshipApplicationService,
    private router: Router,
    private tokenService: TokenService
  ) {}

  get startDate() {
    return this.searchScholarshipForm.get('startDate');
  }

  ngOnInit(): void {
    this.setupForm();
  }

  setupForm() {
    this.searchScholarshipForm = this.formBuilder.group({
      id: [null, ''],
      school: [null, ''],
      description: [null, ''],
      city: [null, ''],
      status: [ApplicationStatusEnum.Submitted, ''],
      value: [null, ''],
      startDate: [null, ''],
      endDate: [null, ''],
    });
  }

  search() {
    this.submitting = true;
    this.isError = false;
    this.scholarshipApplicationService
      .getMyScholarshipApplications(
        <ScholarshipApplicationSearchDTO>this.searchScholarshipForm.value
      )
      .then((response) => {
        this.scholarships.emit(response);
      })
      .catch((response) => {
        console.error(response);
      })
      .finally(() => {
        this.submitting = false;
      });
  }
}
