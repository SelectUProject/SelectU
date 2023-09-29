import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';

import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { DragAndDropService } from 'src/app/services/drag-and-drop/drag-and-drop.service';

@Component({
  selector: 'app-form-creator-area',
  templateUrl: './form-creator-area.component.html',
  styleUrls: ['./form-creator-area.component.scss']
})
export class FormCreatorAreaComponent implements OnInit {
  formSections: ScholarshipFormSectionDTO[];

  constructor(
    private _dragAndDropService: DragAndDropService<ScholarshipFormSectionDTO[]>,
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService
  ) {}

  ngOnInit() {
    this.formSections = this._scholarshipFormSectionListService.formSections;
  }

  onDrop(event: CdkDragDrop<ScholarshipFormSectionDTO[]>): void {
    this._dragAndDropService.drop(event)

    // TODO: Open edit dialog box ONLY if the form builder item was dragged from the Form Components sidebar
    if (event.previousContainer !== event.container) {
      // this.openDialog(event.currentIndex)
      console.log(event)
    }
  }

  deleteFormSection(formSection: ScholarshipFormSectionDTO): void {
    this._scholarshipFormSectionListService.remove(formSection);
  }

  updateFormSectionData(updatedFormSection: ScholarshipFormSectionDTO): void {
    this._scholarshipFormSectionListService.update(updatedFormSection);
  }
}
