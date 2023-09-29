import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
import { MIN_RADIO_BUTTON_INPUTS } from 'src/constants';

@Component({
  selector: 'app-form-section-edit-dialog-box',
  templateUrl: './form-section-edit-dialog-box.component.html',
  styleUrls: ['./form-section-edit-dialog-box.component.scss']
})
export class FormSectionEditDialogBoxComponent {
  formSectionData: ScholarshipFormSectionDTO;

  constructor(
    private dialogRef: MatDialogRef<FormSectionEditDialogBoxComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.formSectionData = data.formSectionData;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  // FOR RADIO BUTTONS (TODO: maybe should be a seperate component)
  isRadioInputFormSection(): boolean {
    return this.formSectionData.type === ScholarshipFormTypeEnum.Option;
  }

  addRadioValue(): void {
    console.log(this.formSectionData);
    if (this.isRadioInputFormSection()) {
      this.formSectionData.options!.push("");
    }
  }

  deleteRadioValue(index: number): void {
    // TODO: Add a message to the users that there has to be more than 2 radio buttons
    if (this.isRadioInputFormSection() && this.isMinimumInputsSatisfied()) {
      this.formSectionData.options!.splice(index, 1);
    }
  }

  // Radio buttons can't have less than 2 inputs. Returns false if the minimum was reached
  isMinimumInputsSatisfied(): boolean {
    return this.formSectionData.options!.length > MIN_RADIO_BUTTON_INPUTS;
  }
}
