import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';
import { MatSnackBar } from '@angular/material/snack-bar';

import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
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

  dateItem: ScholarshipFormSectionDTO = {
    uuid: uuidv4(),
    type: ScholarshipFormTypeEnum.Date,
    name: "Enter your date of birth:",
    required: false,
  }

  fileItem: ScholarshipFormSectionDTO = {
    uuid: uuidv4(),
    type: ScholarshipFormTypeEnum.File,
    name: "Upload your resume:",
    required: true,
  }

  booleanItem: ScholarshipFormSectionDTO = {
    uuid: uuidv4(),
    type: ScholarshipFormTypeEnum.Boolean,
    name: "Are you a local student?",
    required: true,
  }

  radioButtonItem: ScholarshipFormSectionDTO = {
    uuid: uuidv4(),
    type: ScholarshipFormTypeEnum.Option,
    name: "Enter your age:",
    required: true,
    options: ["21-30", "31-40", "41-50", "testing"],
  }

  stringItem: ScholarshipFormSectionDTO = {
    uuid: uuidv4(),
    type: ScholarshipFormTypeEnum.String,
    name: "Tell us about your experience:",
    required: true,
  }

  scholarshipFormSections: ScholarshipFormSectionDTO[] = [
    this.dateItem,
    this.fileItem,
    this.booleanItem,
    this.radioButtonItem,
    this.stringItem
  ]

  setupForm() {
    this.createScholarshipForm = this._formBuilder.group(
      {
        school: ['Test 1', Validators.required],
        imageURL: ['test.png', Validators.required],
        value: ['Something here', Validators.required],
        shortDescription: ['The best scholarship', Validators.required],
        description: ['Some description here', Validators.required],
        scholarshipFormTemplate: [this._scholarshipFormSectionListService.formSections, Validators.required],
        city: ['Melbourne', Validators.required],
        state: ['Victoria', Validators.required],
        startDate: [new Date(), Validators.required],
        endDate: [new Date(), Validators.required]
      }
    );
  }

  createScholarship() {
    let createScholarshipForm = <ScholarshipCreateDTO>this.createScholarshipForm.value;
    console.log(createScholarshipForm);

    // this._scholarshipService
    //   .createScholarship(createScholarshipForm)
    //   .then((response) => {
    //     this._snackBar.open(response.message, 'X', {
    //       duration: 5000
    //     });

    //     this._router.navigate(['/manage-scholarships']);
    //   })
    //   .catch((response) => {
    //     // TODO: add an error snackbar here
    //     console.error(response);
    //   });
  }
}
