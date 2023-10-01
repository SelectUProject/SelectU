import { Component, Input } from '@angular/core';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { TokenService } from 'src/app/providers/token.service';
import { ADMIN, STAFF, USER } from 'src/app/constants/userRoles';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-short-view-scholarships',
  templateUrl: './short-view-scholarships.component.html',
  styleUrls: ['./short-view-scholarships.component.scss'],
})
export class ShortViewScholarshipsComponent {
  ADMIN = ADMIN;
  STAFF = STAFF;
  USER = USER;
  @Input() scholarship: ScholarshipUpdateDTO;

  set data(value: ScholarshipUpdateDTO) {
    this.scholarshipService.scholarship = value;
  }

  constructor(
    private router: Router,
    public tokenService: TokenService,
    public scholarshipService: ScholarshipService
  ) {}
}
