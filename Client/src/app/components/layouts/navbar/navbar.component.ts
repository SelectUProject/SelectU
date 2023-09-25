import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ADMIN, STAFF } from 'src/app/constants/userRoles';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { TokenService } from 'src/app/providers/token.service';
import { UserService } from 'src/app/providers/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
class NavbarComponent {
  @Input() title: string = 'Title';
  admissionName = environment.admissionName;
  ADMIN = ADMIN;
  STAFF = STAFF;
  userDetails: UserUpdateDTO;

  constructor(
    public tokenService: TokenService,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.getUserDetails();
  }

  logout() {
    this.tokenService.clearToken();
    this.router.navigate(['/']);
  }

  async getUserDetails() {
    if (this.tokenService.IsAuthenticated) {
      await this.userService
        .getUserDetails()
        .then((response) => {
          this.userDetails = response;
        })
        .catch((err) => {
          console.log(err);
        });
    }
  }
}

export default NavbarComponent;
