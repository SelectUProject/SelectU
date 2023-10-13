import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';
import { MIN_RADIO_BUTTON_INPUTS } from 'src/app/constants/FormConstraints';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ScholarshipFormTypeEnum } from 'src/app/models/ScholarshipFormTypeEnum';

@Component({
  selector: 'app-form-section-edit-dialog-box',
  templateUrl: './form-section-edit-dialog-box.component.html',
  styleUrls: ['./form-section-edit-dialog-box.component.scss']
})
export class FormSectionEditDialogBoxComponent implements OnInit {
  @Input() formSectionData: ScholarshipFormSectionDTO;
  scholarshipFormSectionFormGroup: FormGroup;

  @Output() passEntry: EventEmitter<any> = new EventEmitter();

  constructor(
    public activeModal: NgbActiveModal,
    private _formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.scholarshipFormSectionFormGroup = this._formBuilder.group(
      {
        uuid: this.formSectionData.uuid,
        type: this.formSectionData.type,
        name: [this.formSectionData.name, Validators.required],
        required: this.formSectionData.required,
        options: this._formBuilder.array(this.formSectionData.options!)
      }
    );
  }

  passBack(): void {
    const normalisedForm = <ScholarshipFormSectionDTO>this.scholarshipFormSectionFormGroup.value;

    this.passEntry.emit(normalisedForm)
    this.activeModal.close(normalisedForm);
  }

  /*
   * The following code is for OPTION form section field management
   */
  isRadioInputFormSection(): boolean {
    return this.formSectionData.type === ScholarshipFormTypeEnum.Option;
  }

  get options(): FormArray {
    return this.scholarshipFormSectionFormGroup.get('options') as FormArray;
  }

  addRadioValue(): void {
    if (this.isRadioInputFormSection()) {
      this.options.push(this._formBuilder.control(''));
    }
  }

  deleteRadioValue(index: number): void {
    if (this.isRadioInputFormSection() && this.isMinimumInputsSatisfied()) {
      this.options.removeAt(index);
    }
  }

  // Radio buttons can't have less than 2 inputs. Returns false if the minimum was reached
  isMinimumInputsSatisfied(): boolean {
    return this.options.length > MIN_RADIO_BUTTON_INPUTS;
  }
}
