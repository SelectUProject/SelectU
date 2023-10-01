import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ToastService } from 'src/app/providers/toast.service';
import { UserService } from 'src/app/providers/user.service';
import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';

@Component({
  selector: 'app-edit-scholarship-page',
  templateUrl: './edit-scholarship-page.component.html',
  styleUrls: ['./edit-scholarship-page.component.scss']
})
export class EditScholarshipPageComponent {
  editScholarshipForm: FormGroup;
  scholarship: ScholarshipUpdateDTO;
  scholarshipCreatorId: string;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _scholarshipService: ScholarshipService,
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService,
    private _userService: UserService,
    private _formBuilder: FormBuilder,
    private _toastService: ToastService
  ) {
    this.setupForm();
  }

  async setupForm() {
    const scholarshipId = this._route.snapshot.paramMap.get('id');

    await this._scholarshipService
      .getScholarshipDetails(scholarshipId)
        .then((response) => {
          this.scholarship = response as unknown as ScholarshipUpdateDTO;
          this._scholarshipFormSectionListService.formSections = this.scholarship.scholarshipFormTemplate;

          this.getUserDetails();
          this.populateForm();
        })
        .catch((response) => {
          this._toastService.show(response.message, { classname: 'bg-danger text-light'} );
          this._router.navigate(['/manage-scholarships']);
        })
  }

  // TODO: move user details to the global file/servce
  async getUserDetails() {
    await this._userService
      .getUserDetails()
      .then((response) => {
        console.log(response.id);
        this.scholarshipCreatorId = response.id;
      })
      .catch((response) => {
        console.log(response);
      });
  }

  populateForm(): void {
    const startDate: Date = new Date(this.scholarship.startDate);
    const endDate: Date = new Date(this.scholarship.endDate);

    // TODO: Add validation here
    this.editScholarshipForm = this._formBuilder.group(
      {
        id: [this.scholarship.id],
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

    // TODO: Fix this
    editScholarshipForm.scholarshipCreatorId = this.scholarshipCreatorId;

    // TODO: Streamline this, need to convert from NgbDate to regular Date bc that's the type
    editScholarshipForm.startDate = new Date(
      (editScholarshipForm.startDate as any).year,
      (editScholarshipForm.startDate as any).month - 1,
      (editScholarshipForm.startDate as any).day
    )

    // TODO: Streamline this, need to convert from NgbDate to regular Date bc that's the type
    editScholarshipForm.endDate = new Date(
      (editScholarshipForm.endDate as any).year,
      (editScholarshipForm.endDate as any).month - 1,
      (editScholarshipForm.endDate as any).day
    )

    this._scholarshipService
      .updateScholarship(editScholarshipForm)
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

  deleteScholarship() {
    const deleteConfirm = confirm(`Are you sure you want to delete this scholarship:\n"${this.scholarship.shortDescription}"?`);

    if (deleteConfirm) {
      this._scholarshipService
        .deleteScholarship(this.scholarship.id)
        .then((response) => {
          this._toastService.show(response.message, {
            classname: 'bg-success text-light',
          });

          this._router.navigate(['/manage-scholarships']);
        })
        .catch((response) => {
          this._toastService.show(response.error, {
            classname: 'bg-danger text-light',
          });
        });
    }
  }
}
