import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

import { ScholarshipCreateDTO } from 'src/app/models/ScholarshipCreateDTO';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';

@Component({
  selector: 'app-create-scholarship',
  templateUrl: './create-scholarship.component.html',
  styleUrls: ['./create-scholarship.component.scss']
})
export class CreateScholarshipComponent implements OnInit {
  createScholarshipForm: FormGroup;

  constructor(
    private _formBuilder: FormBuilder,
    private _router: Router,
    private _scholarshipService: ScholarshipService,
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  // The scholarship creation page should always open Scholarship Information tab first
  currentTab: string = "scholarshipInformationTab";

  // TODO: maybe make this an enum?
  tabs: string[] = ["scholarshipInformationTab", "formBuilderTab", "previewApplicationTab"];

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
        scholarshipFormTemplate: [this._scholarshipFormSectionListService.formSections, Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        startDate: [new Date(), Validators.required],
        endDate: [new Date(), Validators.required]
      }
    );
  }

  createScholarship() {
    let createScholarshipForm = <ScholarshipCreateDTO>this.createScholarshipForm.value;

    this._scholarshipService
      .createScholarship(createScholarshipForm)
      .then((response) => {
        this._snackBar.open(response.message, 'X', {
          duration: 5000
        });

        this._router.navigate(['/manage-scholarships']);
      })
      .catch((response) => {
        // TODO: add an error snackbar here
        console.error(response);
      });
  }
}
