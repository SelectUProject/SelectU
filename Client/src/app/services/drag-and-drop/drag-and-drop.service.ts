import { Injectable } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { cloneDeep } from 'lodash';

@Injectable()
export class DragAndDropService<T extends any[]> {

  drop(event: CdkDragDrop<T>): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex)
    } else if (event.container.id !== "form-builder-components-list") { // don't allow copying to the form components list from the application form builder
      // Each newly added item to the application form should be a deep copy
      const clone = cloneDeep(event.previousContainer.data[event.previousIndex])

      event.container.data.splice(event.currentIndex, 0, clone)
    }
  }
}
