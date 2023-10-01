import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

import { ScholarshipShortViewDTO } from '../../../models/ScholarshipShortViewDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { TokenService } from 'src/app/providers/token.service';

@Component({
  selector: 'app-short-view-scholarships-component',
  templateUrl: './short-view-scholarships-component.component.html',
  styleUrls: ['./short-view-scholarships-component.component.scss'],
})
export class ShortViewScholarshipsComponentComponent {
  @Input() scholarship: ScholarshipUpdateDTO;

  constructor(public tokenService: TokenService, private _router: Router) {}

  editScholarship(): void {
    if (this.tokenService.role === 'Staff') {
      this._router.navigate([`scholarship/${this.scholarship.id}/edit`])
    }
  }
}
