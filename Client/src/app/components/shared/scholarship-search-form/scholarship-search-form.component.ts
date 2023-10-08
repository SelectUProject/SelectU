import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/providers/token.service';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipSearchDTO } from 'src/app/models/ScholarshipSearchDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { StatusEnum } from 'src/app/models/StatusEnum';

@Component({
  selector: 'app-scholarship-search-form',
  templateUrl: './scholarship-search-form.component.html',
  styleUrls: ['./scholarship-search-form.component.scss'],
})
export class ScholarshipSearchFormComponent implements OnInit {
  searchScholarshipForm: FormGroup;
  submitting: boolean = false;
  isError: boolean = false;
  errMsg: string = 'Some error has occurred!';
  // scholarships: ScholarshipUpdateDTO[] = [];
  todayDate: Date = new Date();

  @Output() scholarships = new EventEmitter<ScholarshipUpdateDTO[]>();

  constructor(
    private formBuilder: FormBuilder,
    private scholarshipService: ScholarshipService,
    private router: Router,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  setupForm() {
    this.searchScholarshipForm = this.formBuilder.group({
      id: [null, ''],
      school: [null, ''],
      description: [null, ''],
      city: [null, ''],
      status: [StatusEnum.Pending, ''],
      value: [null, ''],
      startDate: [null, ''],
      endDate: [null, ''],
    });
  }

  search() {
    this.submitting = true;
    this.isError = false;
    this.scholarshipService
      .getActiveScholarships(
        <ScholarshipSearchDTO>this.searchScholarshipForm.value
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
