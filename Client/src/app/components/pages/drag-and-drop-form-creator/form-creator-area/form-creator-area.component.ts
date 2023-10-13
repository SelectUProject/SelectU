import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';

import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { DragAndDropService } from 'src/app/services/drag-and-drop/drag-and-drop.service';
import { FormSectionComponent } from '../form-section/form-section.component';

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
    // Dropping form sectioon to the form creator area
    this._dragAndDropService.drop(event)

    // Generating a new uuid for the form section
    event.container.data[event.currentIndex].uuid = uuidv4();
  }

  deleteFormSection(formSection: ScholarshipFormSectionDTO): void {
    this._scholarshipFormSectionListService.remove(formSection);
  }

  updateFormSectionData(updatedFormSection: ScholarshipFormSectionDTO): void {
    this._scholarshipFormSectionListService.update(updatedFormSection);
  }
}
