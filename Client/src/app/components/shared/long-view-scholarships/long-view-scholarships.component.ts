import { Component, Input } from '@angular/core';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';

@Component({
  selector: 'app-long-view-scholarships',
  templateUrl: './long-view-scholarships.component.html',
  styleUrls: ['./long-view-scholarships.component.scss'],
})
export class LongViewScholarshipsComponent {
  @Input() scholarship: ScholarshipUpdateDTO;
}
