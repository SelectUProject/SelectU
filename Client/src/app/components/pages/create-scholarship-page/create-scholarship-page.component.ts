import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ScholarshipCreateDTO } from 'src/app/models/ScholarshipCreateDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ToastService } from 'src/app/providers/toast.service';
import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';

@Component({
  selector: 'app-create-scholarship-page',
  templateUrl: './create-scholarship-page.component.html',
  styleUrls: ['./create-scholarship-page.component.scss']
})
export class CreateScholarshipPageComponent {
  createScholarshipForm: FormGroup;

  constructor(
    private _router: Router,
    private _scholarshipService: ScholarshipService,
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService,
    private _formBuilder: FormBuilder,
    private _toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.createScholarshipForm = this._formBuilder.group(
      {
        school: new FormControl('', Validators.required),
        imageURL: new FormControl('', Validators.required),
        value: new FormControl('', Validators.required),
        shortDescription: new FormControl('', Validators.required),
        description: new FormControl('', Validators.required),
        scholarshipFormTemplate: [this._scholarshipFormSectionListService.formSections],
        city: new FormControl('', Validators.required),
        state: new FormControl('', Validators.required),
        startDate: new FormControl('', Validators.required),
        endDate: new FormControl('', Validators.required)
      }
    );
  }

  createScholarship() {
    let createScholarshipForm = <ScholarshipCreateDTO>this.createScholarshipForm.value;
    console.log(createScholarshipForm);

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
        this._toastService.show(response.error.message, {
          classname: 'bg-danger text-light',
          delay: 5000
        });
      });
  }
}
