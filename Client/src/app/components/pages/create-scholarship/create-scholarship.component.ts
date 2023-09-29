import { Component, OnInit } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
import { ScholarshipCreateDTO } from 'src/app/models/ScholarshipCreateDTO';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-scholarship',
  templateUrl: './create-scholarship.component.html',
  styleUrls: ['./create-scholarship.component.scss']
})
export class CreateScholarshipComponent implements OnInit {
  createScholarshipForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {}

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
    name: "HAHAHAHAHAHAAHAHA Enter your date of birth:",
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
    this.createScholarshipForm = this.formBuilder.group(
      {
        school: ['Test 1', Validators.required],
        imageURL: ['test.png', Validators.required],
        value: ['Something here', Validators.required],
        shortDescription: ['The best scholarship', Validators.required],
        description: ['The best scholarship everr', Validators.required],
        scholarshipFormTemplate: [[], Validators.required],
        city: ['Kyiv', Validators.required],
        state: ['California', Validators.required],
        startDate: [new Date(), Validators.required],
        endDate: [new Date(), Validators.required]
      }
    );
  }

  async createScholarship() {
    let createScholarshipForm = <ScholarshipCreateDTO>this.createScholarshipForm.value;
    console.log(createScholarshipForm);
  }
}
