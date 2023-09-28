import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-empty-scholarships',
  templateUrl: './empty-scholarships.component.html',
  styleUrls: ['./empty-scholarships.component.scss'],
})
export class EmptyScholarshipsComponent {
  @Input() text: string;
}
