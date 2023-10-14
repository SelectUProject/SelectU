import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { LoginDTO } from 'src/app/models/LoginDTO';
import { AuthService } from 'src/app/providers/auth.service';
import { Location } from '@angular/common';
import { TokenService } from 'src/app/providers/token.service';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { Role } from 'src/app/models/Role';
import { ADMIN } from 'src/app/constants/userRoles';
import { SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss'],
})
class LoginFormComponent implements OnInit {
  loginForm: FormGroup;
  submitting: boolean = false;
  isError: boolean = false;
  errMsg: string = 'Some error has occurred!';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private tokenService: TokenService,
    private socialAuthService: SocialAuthService
  ) {}

  ngOnInit(): void {
    if (this.tokenService.IsAuthenticated) {
      this.router.navigate(['/scholarships']);
    } else {
      this.tokenService.clearToken();
    }
    this.setupForm();
    this.setupSocialAuthService();
  }

  setupSocialAuthService() {
    this.socialAuthService.authState.subscribe((user) => {
      if (!user) return;
      this.googleLogin(user);
    });
  }

  setupForm() {
    this.loginForm = this.formBuilder.group({
      username: [
        '',
        Validators.compose([Validators.required, Validators.email]),
      ],
      password: ['', Validators.required],
    });
  }

  login() {
    this.submitting = true;
    this.isError = false;
    this.authService
      .login(<LoginDTO>this.loginForm.value)
      .then((response) => {
        this.tokenService.setToken(response);
        this.router.navigate(['/scholarships']);
      })
      .catch((response) => {
        this.loginForm.patchValue({ password: '' });
        if (!response.success) {
          this.errMsg = response.error.message;
        }

        this.submitting = false;
        this.isError = true;
      });
  }

  googleLogin(socialUser: SocialUser) {
    this.submitting = true;
    this.isError = false;
    this.authService
      .googleLogin({ IdToken: socialUser.idToken })
      .then((response) => {
        this.tokenService.setToken(response);
        this.router.navigate(['/scholarships']);
      })
      .catch((response) => {
        if (!response.success) {
          this.errMsg = response.error.message;
        }

        this.submitting = false;
        this.isError = true;
      });
  }
}

export default LoginFormComponent;
