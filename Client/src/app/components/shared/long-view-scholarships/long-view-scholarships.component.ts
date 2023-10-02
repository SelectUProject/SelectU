import { Component, Input } from '@angular/core';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-long-view-scholarships',
  templateUrl: './long-view-scholarships.component.html',
  styleUrls: ['./long-view-scholarships.component.scss'],
})
export class LongViewScholarshipsComponent {
  admissionName = environment.admissionName;
  @Input() scholarship: ScholarshipUpdateDTO;
}
