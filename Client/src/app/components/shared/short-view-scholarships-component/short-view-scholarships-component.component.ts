import { Component, Input } from '@angular/core';
import { ScholarshipShortViewDTO } from '../../../models/ScholarshipShortViewDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';

@Component({
  selector: 'app-short-view-scholarships-component',
  templateUrl: './short-view-scholarships-component.component.html',
  styleUrls: ['./short-view-scholarships-component.component.scss'],
})
export class ShortViewScholarshipsComponentComponent {
  @Input() scholarship: ScholarshipUpdateDTO;
}
