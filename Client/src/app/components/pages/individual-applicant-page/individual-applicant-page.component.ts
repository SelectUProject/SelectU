import { Component } from '@angular/core';
import { ScholarshipFormAnswer } from 'src/app/models/JustTesting';

@Component({
  selector: 'app-individual-applicant-page',
  templateUrl: './individual-applicant-page.component.html',
  styleUrls: ['./individual-applicant-page.component.scss']
})
class IndividualApplicantPageComponent {
  imagePath = '../../../../assets/images/deleteThis1.png';
  fullName = 'Sam Smith';
  scholarshipApplication = {
    id: "1",
    scholarshipApplicantId: "88",
    scholarshipId: "SCHO773",
    scholarshipFormAnswer: [
      {
        name: "Name",
        value: "Sam"
      },
      {
        name: "Reason",
        value: "just chillin"
      },
      {
        name: "Birthday",
        value: "01/01/2000"
      }
    ],
    dateCreated: "01/09/2023",
    dateModified: "05/09/2023",
    status: 3,
  }
};

export default IndividualApplicantPageComponent;
