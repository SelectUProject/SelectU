import { Component, OnInit } from '@angular/core';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { UserService } from 'src/app/providers/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-profile-page',
  templateUrl: './user-profile-page.component.html',
  styleUrls: ['./user-profile-page.component.scss'],
})
export class UserProfilePageComponent implements OnInit {
  userDetails: UserUpdateDTO;

  admissionName = environment.admissionName;


  constructor(private userService: UserService) {}

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
  }
}
