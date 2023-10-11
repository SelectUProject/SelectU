import { Injectable } from '@angular/core';
import { ScholarshipFormSectionDTO } from 'src/app/models/ScholarshipFormSectionDTO';

@Injectable({
  providedIn: 'root'
})
export class ScholarshipFormSectionListService {
  public formSections: ScholarshipFormSectionDTO[] = [];

  add(formSection: ScholarshipFormSectionDTO): void {
    this.formSections.push(formSection);
  }

  remove(formSection: ScholarshipFormSectionDTO): void {
    this.formSections.forEach((fS, index) => {
      if (fS.uuid === formSection.uuid) this.formSections.splice(index, 1);
    })
  }

  update(editedFormSection: ScholarshipFormSectionDTO): void {
    this.formSections[
      this.formSections.findIndex(fS => fS.uuid === editedFormSection.uuid)
    ] = editedFormSection;
  }

  clear() {
		this.formSections.splice(0, this.formSections.length);
	}
}
