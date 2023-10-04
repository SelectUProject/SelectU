import { Component, OnInit } from '@angular/core';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { UserService } from 'src/app/providers/user.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-user-profile-page',
  templateUrl: './user-profile-page.component.html',
  styleUrls: ['./user-profile-page.component.scss'],
})
export class UserProfilePageComponent implements OnInit {
  userDetails: UserUpdateDTO;

  gender: string;
  date: string | null;

  

  admissionName = environment.admissionName;

  constructor(private userService: UserService, private router: Router, private datepipe: DatePipe) {}

  ngOnInit(): void {
    this.getUserDetails();
  }

  async getUserDetails() {
    await this.userService
      .getUserDetails()
      .then((response) => {
        this.userDetails = response;
      })
      .catch((response) => {
        console.log(response);
      });

    this.gender = getGenderString(this.userDetails.gender);
    let formated_date =this.datepipe.transform(this.userDetails.dateOfBirth, 'yyyy-MM-dd');
    this.date = formated_date;
  }

  redirect() {
    this.router.navigate(['/update-account']);
  }
}

function getGenderString(genderCode: number): string {
  let genderString: string;

  switch (genderCode) {
    case 0:
      genderString = 'Male';
      break;
    case 1:
      genderString = 'Female';
      break;
    case 2:
      genderString = 'Other';
      break;
    default:
      genderString = 'Unknown';
      break;
  }

  return genderString;
}
