import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ToastService } from 'src/app/providers/toast.service';
import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';

@Component({
  selector: 'app-edit-scholarship-page',
  templateUrl: './edit-scholarship-page.component.html',
  styleUrls: ['./edit-scholarship-page.component.scss']
})
export class EditScholarshipPageComponent implements OnInit {
  scholarshipId = this._route.snapshot.paramMap.get('id');
  editScholarshipForm: FormGroup;
  scholarshipImg: FormData = new FormData();

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _scholarshipService: ScholarshipService,
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService,
    private _formBuilder: FormBuilder,
    private _toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.populateForm();
  }

  initializeForm(): void {
    this.editScholarshipForm = this._formBuilder.group(
      {
        id: new FormControl('', Validators.required),
        scholarshipCreatorId: new FormControl('', Validators.required),
        school: new FormControl('', Validators.required),
        imageURL: new FormControl('', Validators.required),
        value: new FormControl('', Validators.required),
        shortDescription: new FormControl('', Validators.required),
        description: new FormControl('', Validators.required),
        scholarshipFormTemplate: [],
        city: new FormControl('', Validators.required),
        state: new FormControl('', Validators.required),
        startDate: new FormControl('', Validators.required),
        endDate: new FormControl('', Validators.required)
      }
    );
  }

  async populateForm(): Promise<void> {
    await this._scholarshipService
      .getScholarshipDetails(this.scholarshipId)
        .then((response) => {
          // Individually adding the form section to the array
          response.scholarshipFormTemplate.forEach(formSection => {
            this._scholarshipFormSectionListService.add(formSection);
          });

          // Updating form values from the response
          this.editScholarshipForm.patchValue(
            {
              id: response.id,
              scholarshipCreatorId: response.scholarshipCreatorId,
              school: response.school,
              imageURL: response.imageURL,
              value: response.value,
              shortDescription: response.shortDescription,
              scholarshipFormTemplate: this._scholarshipFormSectionListService.formSections,
              description: response.description,
              city: response.city,
              state: response.state,
              startDate: new Date(response.startDate),
              endDate: new Date(response.endDate)
            }
          );
        })
        .catch((response) => {
          this._toastService.show(response.message, { classname: 'bg-danger text-light'} );
          this._router.navigate(['/manage-scholarships']);
        })
  }

  updateScholarship() {
    let editScholarshipForm = <ScholarshipUpdateDTO>this.editScholarshipForm.value;

    this._scholarshipService
      .updateScholarship(editScholarshipForm)
      .then((response) => {
        // uploading an image after the scholarship has been created
        this._scholarshipService
          .uploadScholarshipImg(this.scholarshipId, this.scholarshipImg);

        this._toastService.show(response.message, {
          classname: 'bg-success text-light',
          delay: 5000
        });

        this._router.navigate(['/manage-scholarships']);
      })
      .catch((response) => {
        this._toastService.show(response.error.message ?? response.error, {
          classname: 'bg-danger text-light',
          delay: 5000
        });
      });
  }

  deleteScholarship() {
    const deleteConfirm = confirm(`Are you sure you want to delete this scholarship:\n"${this.editScholarshipForm.get("shortDescription")?.value}"?`);

    if (deleteConfirm) {
      this._scholarshipService
        .deleteScholarship(this.scholarshipId)
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
