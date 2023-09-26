import { Component } from '@angular/core';
import { ScholarshipShortViewDTO } from 'src/app/models/ScholarshipShortViewDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-saved-scholarships-page',
  templateUrl: './saved-scholarships-page.component.html',
  styleUrls: ['./saved-scholarships-page.component.scss'],
})
export class SavedScholarshipsPageComponent {
  admissionName = environment.admissionName;
  emptyText = `You have no ${this.admissionName} saved.`;
  scholarships: ScholarshipUpdateDTO[] = [];
}
