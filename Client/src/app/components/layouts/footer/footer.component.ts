import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/providers/token.service';
import { UserService } from 'src/app/providers/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  constructor(
    public tokenService: TokenService,
    private router: Router,
    private userService: UserService
  ) {}

  logout() {
    this.tokenService.clearToken();
    this.router.navigate(['/']);
  }
}
