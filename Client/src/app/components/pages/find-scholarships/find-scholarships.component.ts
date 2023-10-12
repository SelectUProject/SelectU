import { Component } from '@angular/core';
import { ScholarshipSearchDTO } from 'src/app/models/ScholarshipSearchDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { TokenService } from 'src/app/providers/token.service';
import { environment } from 'src/environments/environment';
import { USER } from 'src/app/constants/userRoles';

@Component({
  selector: 'app-find-scholarships',
  templateUrl: './find-scholarships.component.html',
  styleUrls: ['./find-scholarships.component.scss'],
})
export class FindScholarshipsComponent {
  admissionName = environment.admissionName;
  emptyText = `No active ${this.admissionName} found.`;
  scholarships: ScholarshipUpdateDTO[] = [];
  scholarshipSearchDTO: ScholarshipSearchDTO = {};
  USER = USER;

  constructor(
    private scholarshipService: ScholarshipService,
    public tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.tokenService.role == USER
      ? this.getActiveScholarships()
      : this.getAllScholarships();
  }

  async handleSearchEvent(scholarships: ScholarshipUpdateDTO[]) {
    this.scholarships = scholarships;
  }

  getActiveScholarships() {
    this.scholarshipService
      .getActiveScholarships(this.scholarshipSearchDTO)
      .then((response) => {
        this.scholarships = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }

  getAllScholarships() {
    this.scholarshipService
      .getAllScholarships(this.scholarshipSearchDTO)
      .then((response) => {
        this.scholarships = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }
}
