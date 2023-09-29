import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, Input } from '@angular/core';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { DragAndDropService } from 'src/app/services/drag-and-drop/drag-and-drop.service';

@Component({
  selector: 'app-form-creator-area',
  templateUrl: './form-creator-area.component.html',
  styleUrls: ['./form-creator-area.component.scss']
})
export class FormCreatorAreaComponent {
  @Input() formSections: ScholarshipFormSectionDTO[];

  constructor(
    private dragAndDropService: DragAndDropService<ScholarshipFormSectionDTO[]>
  ) {}

  onDrop(event: CdkDragDrop<ScholarshipFormSectionDTO[]>): void {
    this.dragAndDropService.drop(event)

    // TODO: Open edit dialog box ONLY if the form builder item was dragged from the Form Components sidebar
    if (event.previousContainer !== event.container) {
      // this.openDialog(event.currentIndex)
      console.log(event)
    }
  }

  deleteFormSection(formSection: ScholarshipFormSectionDTO): void {
    this.formSections = this.formSections.filter(fS => fS.uuid !== formSection.uuid);
  }
}
