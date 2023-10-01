import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ToastService } from 'src/app/providers/toast.service';
import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';

@Component({
  selector: 'app-edit-scholarship-page',
  templateUrl: './edit-scholarship-page.component.html',
  styleUrls: ['./edit-scholarship-page.component.scss']
})
export class EditScholarshipPageComponent {
  editScholarshipForm: FormGroup;
  scholarship: ScholarshipUpdateDTO;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _scholarshipService: ScholarshipService,
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService,
    private _formBuilder: FormBuilder,
    private _toastService: ToastService
  ) {
    this.setupForm();
  }

  setupForm(): void {
    const scholarshipId = this._route.snapshot.paramMap.get('id') as string;

    this._scholarshipService
      .getScholarshipDetails(JSON.stringify(scholarshipId))
        .then((response) => {
          this.scholarship = response as unknown as ScholarshipUpdateDTO;
          this._scholarshipFormSectionListService.formSections = this.scholarship.scholarshipFormTemplate;

          this.populateForm();
        })
        .catch((response) => {
          console.log(response);
        })
  }

  populateForm(): void {
    const startDate: Date = new Date(this.scholarship.startDate);
    const endDate: Date = new Date(this.scholarship.endDate);

    // TODO: Add validation here
    this.editScholarshipForm = this._formBuilder.group(
      {
        school: [this.scholarship.school],
        imageURL: [this.scholarship.imageURL],
        value: [this.scholarship.value],
        shortDescription: [this.scholarship.shortDescription],
        description: [this.scholarship.description],
        scholarshipFormTemplate: [this._scholarshipFormSectionListService.formSections],
        city: [this.scholarship.city],
        state: [this.scholarship.state],
        startDate: [new NgbDate(startDate.getFullYear(), startDate.getMonth(), startDate.getDate())],
        endDate: [new NgbDate(endDate.getFullYear(), endDate.getMonth(), endDate.getDate())]
      }
    );
  }

  updateScholarship() {
    let editScholarshipForm = <ScholarshipUpdateDTO>this.editScholarshipForm.value;

    console.log(editScholarshipForm);

    // TODO: Add an API call to patch scholarship data

    // this._scholarshipService
    //   .updateScholarship(editScholarshipForm)
    //   .then((response) => {
    //     this._toastService.show(response.message, {
    //       classname: 'bg-success text-light',
    //       delay: 5000
    //     });

    //     this._router.navigate(['/manage-scholarships']);
    //   })
    //   .catch((response) => {
    //     this._toastService.show(response.message, {
    //       classname: 'bg-danger text-light',
    //       delay: 5000
    //     });
    //   });
  }

  deleteScholarship() {
    alert(`Are you sure you want to delete this scholarship:\n"${this.scholarship.shortDescription}"?`)

    // TODO: Add an API call to delete scholarship
  }
}
