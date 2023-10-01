import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';

@Component({
  selector: 'app-create-scholarship-application-page',
  templateUrl: './create-scholarship-application-page.component.html',
  styleUrls: ['./create-scholarship-application-page.component.scss'],
})
export class CreateScholarshipApplicationPageComponent implements OnInit {
  get scholarship(): ScholarshipUpdateDTO {
    return this.scholarshipService.scholarship;
  }
  set scholarship(value: ScholarshipUpdateDTO) {
    this.scholarshipService.scholarship = value;
  }

  constructor(
    public scholarshipService: ScholarshipService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!this.scholarshipService.scholarship) {
      this.router.navigate(['/find-scholarships']);
    }
    console.log(this.scholarshipService.scholarship);
  }
}
