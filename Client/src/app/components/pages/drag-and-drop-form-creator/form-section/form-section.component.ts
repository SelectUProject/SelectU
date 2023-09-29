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

  @Output() destroy: EventEmitter<ScholarshipFormSectionDTO> = new EventEmitter();
  @Output() isInApplicationForm: EventEmitter<boolean> = new EventEmitter();

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
      }
    })
  }

  destroyItself(): void {
    this.destroy.emit(this.formSectionData);
  }

  // For form section that has the enum type of ScholarshipFormTypeEnum.Option
  // removeFormSectionOptionValue(index: number): void {
  //   if (this.isOptionFormSection()) {
  //     this.formSectionData.options?.splice(index, 1);
  //   }
  // }

  isOptionFormSection(): boolean {
    return this.formSectionData.type === ScholarshipFormTypeEnum.Option;
  }

  isInApplicationFormEmitter(): void {
    this.isInApplicationForm.emit(true);
  }

  // onDrop(event: CdkDragDrop<ScholarshipFormSectionDTO[]>): void {
  //   this.dragAndDropService.drop(event)

  //   // Open edit dialog box ONLY if the form builder item was dragged from the Form Components sidebar
  //   if (event.previousContainer !== event.container) {
  //     this.openDialog(event.currentIndex)
  //   }
  // }
}
