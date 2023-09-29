import { Component, Output, EventEmitter } from '@angular/core';
import { environment } from 'src/environments/environment';

// import { ScholarshipFormBuilderItem } from 'src/app/models/form-builder/ScholarshipFormBuilderItem';

import { DragAndDropService } from 'src/app/services/drag-and-drop/drag-and-drop.service';
import { CdkDragDrop } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent {
  // constructor(private dragAndDropService: DragAndDropService<ScholarshipFormBuilderItem[]>) { }

  // scholarshipFormBuilderItems: ScholarshipFormBuilderItem[] = [
  //   {
  //     itemDisplayName: "Text Input Field",
  //     itemType: "textInput",
  //     fieldName: "",
  //     description: "",
  //     isRequired: true,
  //     status: "new",
  //     customAttributes: {}
  //   },
  //   {
  //     itemDisplayName: "Date Picker",
  //     itemType: "datePicker",
  //     fieldName: "",
  //     description: "",
  //     isRequired: true,
  //     status: "new",
  //     customAttributes: {}
  //   },
  // ];

  // isOpen: boolean = true;

  // toggleSidebar() {
  //   this.isOpen = !this.isOpen;
  // }

  // onDrop(event: CdkDragDrop<ScholarshipFormBuilderItem[]>) : void {
  //   this.dragAndDropService.drop(event);
  // }
}
