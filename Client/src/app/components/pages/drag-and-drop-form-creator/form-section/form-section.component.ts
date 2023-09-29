import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { cloneDeep } from 'lodash';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
import { FormSectionEditDialogBoxComponent } from '../form-section-edit-dialog-box/form-section-edit-dialog-box.component';

@Component({
  selector: 'app-form-section',
  templateUrl: './form-section.component.html',
  styleUrls: ['./form-section.component.scss']
})
export class FormSectionComponent {
  @Input() formSectionData: ScholarshipFormSectionDTO;
  @Input() isInApplicationForm: boolean; // to check if the form section is dragged inside of an application form

  @Output() destroy: EventEmitter<ScholarshipFormSectionDTO> = new EventEmitter();
  @Output() updateFormSectionData: EventEmitter<ScholarshipFormSectionDTO> = new EventEmitter();

  constructor(private editDialog: MatDialog) {}

  getFormSectionTypeDisplayName(): string {
    return `${ScholarshipFormTypeEnum[this.formSectionData.type]} Input`;
  }

  openFormSectionEditDialog(): void {
    const dialogRef = this.editDialog.open(FormSectionEditDialogBoxComponent, {
      data: {
        // Passing the deep cloned form section data
        formSectionData: cloneDeep(this.formSectionData),
      },
      disableClose: true,
    })

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        // Updating the current form section data with the user edited information
        this.formSectionData = result;
        this.updateFormSectionData.emit(this.formSectionData);
      }
    })
  }

  destroyItself(): void {
    this.destroy.emit(this.formSectionData);
  }

  isOptionFormSection(): boolean {
    return this.formSectionData.type === ScholarshipFormTypeEnum.Option;
  }
}
