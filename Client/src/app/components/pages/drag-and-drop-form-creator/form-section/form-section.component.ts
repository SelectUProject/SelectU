import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { cloneDeep } from 'lodash';
import { v4 as uuidv4 } from 'uuid';

import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
import { FormSectionEditDialogBoxComponent } from '../form-section-edit-dialog-box/form-section-edit-dialog-box.component';

@Component({
  selector: 'app-form-section',
  templateUrl: './form-section.component.html',
  styleUrls: ['./form-section.component.scss']
})
export class FormSectionComponent implements OnInit {
  @Input() formSectionData: ScholarshipFormSectionDTO;
  @Input() isInApplicationForm: boolean; // to check if the form section is dragged inside of an application form

  @Output() destroy: EventEmitter<ScholarshipFormSectionDTO> = new EventEmitter();
  @Output() updateFormSectionData: EventEmitter<ScholarshipFormSectionDTO> = new EventEmitter();

  constructor(private _editModalService: NgbModal) {}

  ngOnInit() {
    // Generates a unique ID when the form section is created
    this.formSectionData.uuid = uuidv4();
  }

  getFormSectionTypeDisplayName(): string {
    return `${ScholarshipFormTypeEnum[this.formSectionData.type]} Input`;
  }

  openFormSectionEditDialog(): void {
    const modalRef = this._editModalService.open(FormSectionEditDialogBoxComponent, {
      // Ensuring that user can't close the modal box by clicking outside of it
      backdrop: "static",
      keyboard: false
    });

    // Passing the deep cloned form section data
    modalRef.componentInstance.formSectionData = cloneDeep(this.formSectionData);

    modalRef.result.then((result) => {
      if (result) {
        // Updating the current form section data with the user edited information
        this.formSectionData = result;
        this.updateFormSectionData.emit(this.formSectionData);
      }
    });
  }

  destroyItself(): void {
    this.destroy.emit(this.formSectionData);
  }

  isOptionFormSection(): boolean {
    return this.formSectionData.type === ScholarshipFormTypeEnum.Option;
  }
}
