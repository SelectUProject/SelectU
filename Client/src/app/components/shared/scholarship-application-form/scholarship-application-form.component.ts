//
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum'; // Import your enum
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipApplicationService } from 'src/app/providers/ScholarshipApplicationService';
import { ScholarshipApplicationCreateDTO } from 'src/app/models/ScholarshipApplicationCreateDTO';

@Component({
  selector: 'app-scholarship-application-form',
  templateUrl: './scholarship-application-form.component.html',
  styleUrls: ['./scholarship-application-form.component.scss'],
})
export class ScholarshipApplicationFormComponent implements OnInit {
  @Input() scholarship: ScholarshipUpdateDTO;
  scholarshipForm: FormGroup;
  ScholarshipFormTypeEnum = ScholarshipFormTypeEnum; // Make enum available in the template
  submitting: boolean = false;
  isError: boolean;

  constructor(
    private fb: FormBuilder,
    private scholarshipApplicationService: ScholarshipApplicationService
  ) {}

  ngOnInit(): void {
    this.scholarshipForm = this.createScholarshipForm();
  }

  createScholarshipForm(): FormGroup {
    const formControls: { [key: string]: any } = {};

    console.log('scholarship:', this.scholarship);

    if (this.scholarship && this.scholarship.scholarshipFormTemplate) {
      console.log(
        'scholarshipFormTemplate:',
        this.scholarship.scholarshipFormTemplate
      );

      for (const section of this.scholarship.scholarshipFormTemplate) {
        formControls[section.name] = [null]; // Initialize with null value
      }
    }

    console.log('formControls:', formControls);

    return this.fb.group(formControls);
  }

  apply() {
    // Check if the form is valid before submitting
    if (this.scholarshipForm.valid) {
      this.submitting = true;
      this.isError = false;

      // Create a data object to hold the form values
      const formData = this.scholarshipForm.value;

      // You can optionally log the form data to the console for debugging
      console.log('Form Data:', this.scholarshipForm);
      console.log('Form Data:', formData);

      // Assuming you have a service called scholarshipApplicationService to handle the submission
      this.scholarshipApplicationService
        .createScholarshipApplications(
          <ScholarshipApplicationCreateDTO>formData
        )
        .then((response) => {
          // Handle success response from the backend
          console.log('Submission Successful:', response);
          // Optionally, reset the form or perform other actions
          this.scholarshipForm.reset();
        })
        .catch((error) => {
          // Handle error response from the backend
          console.error('Submission Error:', error);
          // Set an error flag or display an error message
          this.isError = true;
        })
        .finally(() => {
          this.submitting = false;
        });
    }
  }
}
