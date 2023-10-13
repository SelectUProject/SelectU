import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { Form, FormGroup } from '@angular/forms';

import { ScholarshipFormSectionListService } from 'src/app/services/scholarship-form-section-list/scholarship-form-section-list.service';
import { StringLookupDTO } from 'src/app/models/LookupDTOs';
import { STATES_LIST } from 'src/app/constants/States';
import { ScholarshipService } from 'src/app/providers/scholarship.service';

@Component({
  selector: 'app-scholarship-editor',
  templateUrl: './scholarship-editor.component.html',
  styleUrls: ['./scholarship-editor.component.scss']
})
export class ScholarshipEditorComponent implements OnDestroy {
  @Input() newScholarship: boolean = false;
  @Input() scholarshipFormGroup: FormGroup;
  @Input() scholarshipImg: FormData;

  @Output() updateButtonClick: EventEmitter<void> = new EventEmitter();
  @Output() deleteButtonClick: EventEmitter<void> = new EventEmitter();
  @Output() createButtonClick: EventEmitter<void> = new EventEmitter();

  file: any;
  states: StringLookupDTO[] = STATES_LIST;

  constructor(
    private _scholarshipFormSectionListService: ScholarshipFormSectionListService,
    private _scholarshipService: ScholarshipService
  ) {}

  ngOnDestroy(): void {
		this._scholarshipFormSectionListService.clear();
	}

  // The scholarship creation page should always open Scholarship Information tab first
  tabs: string[] = ["details", "form builder"];
  currentTab: string = this.tabs[0];

  switchTabTo(tabName: string): void {
    this.currentTab = tabName;
  }

  handleImageFile(event: any): void {
    // this._scholarshipService.uploadScholarshipImg(this.scholarshipFormGroup.)

    this.scholarshipImg.append('file', event.target.files[0]);

    // this.scholarshipFormGroup.patchValue({ imageURL: formData });
    // this.scholarshipFormGroup.updateValueAndValidity();
  }

  emitUpdateButtonClick() {
    this.updateButtonClick.emit();
  }

  emitDeleteButtonClick() {
    this.deleteButtonClick.emit();
  }

  emitCreateButtonClick() {
    this.createButtonClick.emit();
  }
}
