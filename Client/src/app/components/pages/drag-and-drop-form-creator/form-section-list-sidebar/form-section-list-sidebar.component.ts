import { Component } from '@angular/core';
import { v4 as uuidv4 } from 'uuid';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';
import { DragAndDropService } from 'src/app/services/drag-and-drop/drag-and-drop.service';
import { CdkDragDrop } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-form-section-list-sidebar',
  templateUrl: './form-section-list-sidebar.component.html',
  styleUrls: ['./form-section-list-sidebar.component.scss']
})
export class FormSectionListSidebarComponent {
  formSections: ScholarshipFormSectionDTO[] = [];

  constructor(private dragAndDropService: DragAndDropService<ScholarshipFormSectionDTO[]>) {
    this.setupFormSections()
  }

  setupFormSections(): void {
    for (const formSectionType in ScholarshipFormTypeEnum) {
      const formSection: ScholarshipFormSectionDTO = {
        uuid: uuidv4(),
        type: ScholarshipFormTypeEnum[formSectionType as keyof typeof ScholarshipFormTypeEnum],
        name: "",
        required: true,
        options: [],
      }

      this.formSections.push(formSection);
    }
  }

  onDrop(event: CdkDragDrop<ScholarshipFormSectionDTO[]>) : void {
    this.dragAndDropService.drop(event);

    // TODO: Generate a new uuid so that the next form section input is unique
  }
}
