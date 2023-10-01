import { Component, Input } from '@angular/core';
import { ScholarshipShortViewDTO } from '../../../models/ScholarshipShortViewDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { TokenService } from 'src/app/providers/token.service';
import { ADMIN, STAFF, USER } from 'src/app/constants/userRoles';

@Component({
  selector: 'app-short-view-scholarships-component',
  templateUrl: './short-view-scholarships-component.component.html',
  styleUrls: ['./short-view-scholarships-component.component.scss'],
})
export class ShortViewScholarshipsComponentComponent {
  ADMIN = ADMIN;
  STAFF = STAFF;
  USER = USER;
  @Input() scholarship: ScholarshipUpdateDTO;

  constructor(public tokenService: TokenService) {}
}
