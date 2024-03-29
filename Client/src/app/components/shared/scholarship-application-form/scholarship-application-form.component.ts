//
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum'; // Import your enum
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipApplicationService } from 'src/app/providers/application.service';
import { ScholarshipApplicationCreateDTO } from 'src/app/models/ScholarshipApplicationCreateDTO';
import { ScholarshipFormSectionAnswerDTO } from 'src/app/models/ScholarshipFormSectionAnswerDTO';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-scholarship-application-form',
  templateUrl: './scholarship-application-form.component.html',
  styleUrls: ['./scholarship-application-form.component.scss'],
})
export class ScholarshipApplicationFormComponent implements OnInit {
  uploaded: { endpoint: string; key: any }[] = [];
  admissionName = environment.admissionName;
  @Input() scholarship: ScholarshipUpdateDTO;
  scholarshipForm: FormGroup;
  ScholarshipFormTypeEnum = ScholarshipFormTypeEnum; // Make enum available in the template
  submitting: boolean = false;
  isError: boolean;
  success: boolean;
  errMsg: string;

  constructor(
    private fb: FormBuilder,
    private scholarshipApplicationService: ScholarshipApplicationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.scholarshipForm = this.createScholarshipForm();
  }

  createScholarshipForm(): FormGroup {
    const formControls: { [key: string]: any } = {};

    if (this.scholarship && this.scholarship.scholarshipFormTemplate) {
      for (const section of this.scholarship.scholarshipFormTemplate) {
        formControls[section.name] = [null]; // Initialize with null value
      }
    }

    return this.fb.group(formControls);
  }

  handleFileInput(files: FileList, key: any) {
    if (files.length > 0) {
      const file: File = files.item(0)!;

      const formData = new FormData();

      formData.append('file', file);

      this.scholarshipApplicationService
        .uploadFile(formData)
        .then((response) => {
          let newObject = {
            endpoint: response.fileUri,
            key: key,
          };

          this.uploaded.push(newObject);
        })
        .catch((response) => {
          console.error(response);
        });
    }
  }

  apply() {
    // Check if the form is valid before submitting
    if (this.scholarshipForm.valid) {
      this.submitting = true;
      this.isError = false;

      // Create a data object to hold the form values
      const formData = this.scholarshipForm.value;

      const formAnswers: ScholarshipFormSectionAnswerDTO[] = [];

      // Loop through formData and create ScholarshipFormSectionAnswerDTO objects
      for (const key in formData) {
        if (formData.hasOwnProperty(key)) {
          let formType = this.scholarship.scholarshipFormTemplate.find(
            (x) => x.name == key
          )!.type;
          if (formType == ScholarshipFormTypeEnum.File) {
            const foundObject = this.uploaded.find((item) => item.key === key);

            if (foundObject) {
              const answer: ScholarshipFormSectionAnswerDTO = {
                name: key,
                type: formType,
                value: foundObject?.endpoint,
              };
              formAnswers.push(answer);
              // Now 'file' contains the file associated with the specified key
            } else {
              // Handle the case where the key is not found in the array
              console.error('Key not found in the uploaded array');
            }
          } else {
            const answer: ScholarshipFormSectionAnswerDTO = {
              name: key,
              type: formType,
              value: formData[key],
            };
            formAnswers.push(answer);
          }
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
          this.success = true;
          this.router.navigate(['/my-applications']);
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
