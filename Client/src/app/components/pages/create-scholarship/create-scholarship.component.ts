import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/providers/toast.service';

import { ScholarshipCreateDTO } from 'src/app/models/ScholarshipCreateDTO';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';
import { StringLookupDTO } from 'src/app/models/LookupDTOs';
import { STATES_LIST } from 'src/app/constants/States';

@Component({
  selector: 'app-create-scholarship',
  templateUrl: './create-scholarship.component.html',
  styleUrls: ['./create-scholarship.component.scss']
})
export class CreateScholarshipComponent implements OnInit, OnDestroy {
  file: any;
  createScholarshipForm: FormGroup;
  states: StringLookupDTO[] = STATES_LIST;

  constructor(
    private _router: Router,
    private _scholarshipService: ScholarshipService,
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService,
    private _formBuilder: FormBuilder,
    private _toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  ngOnDestroy(): void {
		this._scholarshipFormSectionListService.clear();
	}

  // The scholarship creation page should always open Scholarship Information tab first
  tabs: string[] = ["scholarshipInformationTab", "formBuilderTab"];
  currentTab: string = this.tabs[0];

  switchTabTo(tabName: string): void {
    this.currentTab = tabName;
  }

  setupForm() {
    this.createScholarshipForm = this._formBuilder.group(
      {
        school: ['', Validators.required],
        imageURL: ['', Validators.required],
        value: ['', Validators.required],
        shortDescription: ['', Validators.required],
        description: ['', Validators.required],
        // TODO: Fix the validation for formSections array, the array doesn't get updated when using this._formBuilder.array(this._scholarshipFormSectionListService.formSections, Validators.required)
        scholarshipFormTemplate: [this._scholarshipFormSectionListService.formSections],
        city: ['', Validators.required],
        state: ['', Validators.required],
        startDate: ['', Validators.required],
        endDate: ['', Validators.required]
      }
    );
  }

  createScholarship() {
    let createScholarshipForm = <ScholarshipCreateDTO>this.createScholarshipForm.value;

    this._scholarshipService
      .createScholarship(createScholarshipForm)
      .then((response) => {
        this._toastService.show(response.message, {
          classname: 'bg-success text-light',
          delay: 5000
        });

        this._router.navigate(['/manage-scholarships']);
      })
      .catch((response) => {
        this._toastService.show(response.message, {
          classname: 'bg-danger text-light',
          delay: 5000
        });
      });
  }
}
