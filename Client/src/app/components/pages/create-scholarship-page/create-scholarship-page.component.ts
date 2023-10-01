import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
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
    this.setupForm();
  }

  setupForm() {
    this.createScholarshipForm = this._formBuilder.group(
      {
        school: ['', Validators.required],
        imageURL: ['', Validators.required],
        value: ['', Validators.required],
        shortDescription: ['', Validators.required],
        description: ['', Validators.required],
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

    // TODO: Streamline this, need to convert from NgbDate to regular Date bc that's the type
    createScholarshipForm.startDate = new Date(
      (createScholarshipForm.startDate as any).year,
      (createScholarshipForm.startDate as any).month - 1,
      (createScholarshipForm.startDate as any).day
    )

    // TODO: Streamline this, need to convert from NgbDate to regular Date bc that's the type
    createScholarshipForm.endDate = new Date(
      (createScholarshipForm.endDate as any).year,
      (createScholarshipForm.endDate as any).month - 1,
      (createScholarshipForm.endDate as any).day
    )

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
