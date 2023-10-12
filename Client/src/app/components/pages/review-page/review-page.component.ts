import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipApplicationService } from 'src/app/providers/application.service';
import { TokenService } from 'src/app/providers/token.service';

@Component({
  selector: 'app-review-page',
  templateUrl: './review-page.component.html',
  styleUrls: ['./review-page.component.scss'],
})
class ReviewPageComponent implements OnInit {
  scholarshipId: string;
  application: ScholarshipApplicationUpdateDTO;

  constructor(
    private tokenService: TokenService,
    private route: ActivatedRoute,
    private applicationService: ScholarshipApplicationService,
    private router: Router
  ) {
    this.scholarshipId = this.route.snapshot.url[1].path;
  }

  ngOnInit(): void {
    this.getApplication();
  }

  getApplication() {
    this.applicationService
      .getNextReviewableApplicationAsync(this.scholarshipId)
      .then((response) => {
        this.application = response;
      })
      .catch((response) => {
        this.router.navigate([`/applications/${this.scholarshipId}`]);
        console.error(response);
      });
  }

  handleSuccess() {
    this.getApplication();
  }
}

export default ReviewPageComponent;
