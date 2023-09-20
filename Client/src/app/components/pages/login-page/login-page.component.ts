import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/providers/token.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
class LoginPageComponent implements OnInit {
  constructor(private router: Router, private tokenService: TokenService) {}

  ngOnInit(): void {}
}

export default LoginPageComponent;
