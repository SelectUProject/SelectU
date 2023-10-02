//
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum'; // Import your enum
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipApplicationService } from 'src/app/providers/ScholarshipApplicationService';
import { ScholarshipApplicationCreateDTO } from 'src/app/models/ScholarshipApplicationCreateDTO';
import { ScholarshipFormSectionAnswerDTO } from 'src/app/models/ScholarshipFormSectionAnswerDTO';

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
  errMsg: string;

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

      const formAnswers: ScholarshipFormSectionAnswerDTO[] = [];

      // Loop through formData and create ScholarshipFormSectionAnswerDTO objects
      for (const key in formData) {
        if (formData.hasOwnProperty(key)) {
          const answer: ScholarshipFormSectionAnswerDTO = {
            name: key,
            value: formData[key],
          };
          formAnswers.push(answer);
        }
      }

      let formAnswer: ScholarshipApplicationCreateDTO = {
        scholarshipId: this.scholarship.id,
        scholarshipFormAnswer: formAnswers,
      };

      // Assuming you have a service called scholarshipApplicationService to handle the submission
      this.scholarshipApplicationService
        .createScholarshipApplications(formAnswer)
        .then((response) => {
          // Handle success response from the backend
          console.log('Submission Successful:', response);
          // Optionally, reset the form or perform other actions
          this.scholarshipForm.reset();
        })
        .catch((response) => {
          if (response.error.errors) {
            this.errMsg = 'One or more validation errors occurred.';
            // response.error.errors?.forEach((form: any) => {
            //   this.setFormError(form.propertyName);
            // });
          } else if (!response.success) {
            this.errMsg = response.error.message;
          }
          this.isError = true;
        })
        .finally(() => {
          this.submitting = false;
        });
    }
  }

  setFormError(propertyName: string) {
    //fix
    // propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    // this.scholarshipForm.controls[propertyName]?.setErrors({ required: true });
  }
}
