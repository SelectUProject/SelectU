import {
  GoogleLoginProvider,
  SocialAuthService,
  SocialUser,
} from '@abacritt/angularx-social-login';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { GENDERS_LIST } from 'src/app/constants/Genders';
import { STATES_LIST } from 'src/app/constants/States';
import { NumberLookupDTO, StringLookupDTO } from 'src/app/models/LookupDTOs';
import { Role } from 'src/app/models/Role';
import { UserRegisterDTO } from 'src/app/models/UserRegisterDTO';
import { ValidateUniqueEmailAddressRequestDTO } from 'src/app/models/ValidateUniqueEmailAddressDTO';
import { AuthService } from 'src/app/providers/auth.service';
import { TokenService } from 'src/app/providers/token.service';
import { UserService } from 'src/app/providers/user.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss'],
})
class RegisterFormComponent implements OnInit {
  genders: NumberLookupDTO[] = GENDERS_LIST;
  states: StringLookupDTO[] = STATES_LIST;
  registerForm: FormGroup;
  saving: boolean = false;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  existingEmail: boolean = false;
  todayDate: Date = new Date();
  registered: boolean = false;

  get email() {
    return this.registerForm.get('email');
  }

  get firstName() {
    return this.registerForm.get('firstName');
  }

  get lastName() {
    return this.registerForm.get('lastName');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  get dateOfBirth() {
    return this.registerForm.get('dateOfBirth');
  }

  get gender() {
    return this.registerForm.get('gender');
  }

  get phoneNumber() {
    return this.registerForm.get('phoneNumber');
  }

  get state() {
    return this.registerForm.get('state');
  }

  get country() {
    return this.registerForm.get('country');
  }

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router,
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
      this.googleRegister(user);
    });
  }

  setupForm() {
    this.registerForm = this.formBuilder.group(
      {
        email: [
          '',
          Validators.compose([Validators.required, Validators.email]),
        ],
        firstName: [
          '',
          Validators.compose([
            Validators.required,
            Validators.pattern('^[a-zA-Z ]+$'),
          ]),
        ],
        lastName: [
          '',
          Validators.compose([
            Validators.required,
            Validators.pattern('^[a-zA-Z ]+$'),
          ]),
        ],
        password: [
          '',
          [
            Validators.required,
            Validators.pattern(
              '(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=[^0-9]*[0-9]).{8,}'
            ),
          ],
        ],
        confirmPassword: ['', [Validators.required]],
        dateOfBirth: ['', Validators.required],
        gender: ['', Validators.required],
        phoneNumber: [
          '',
          Validators.compose([
            Validators.required,
            Validators.pattern('^[0-9]+$'),
          ]),
        ],
        state: ['', Validators.required],
        country: ['', Validators.required],
      },
      { validators: [this.matchingPasswords('password', 'confirmPassword')] }
    );
  }

  matchingPasswords(passwordKey: string, confirmPasswordKey: string) {
    return (control: AbstractControl) => {
      const password = control.get(passwordKey)?.value;
      const confirmPassword = control.get(confirmPasswordKey)?.value;

      if (password !== confirmPassword) {
        control
          .get('confirmPassword')
          ?.setErrors({ passwordsDoNotMatch: true });

        return { passwordsDoNotMatch: true };
      }

      return null;
    };
  }

  async register() {
    this.saving = true;
    this.isError = false;
    let registerForm = <UserRegisterDTO>this.registerForm.value;

    await this.userService
      .register(registerForm)
      .then(async () => {
        await this.authService
          .login({
            username: registerForm.email,
            password: registerForm.password,
          })
          .then((response) => {
            this.tokenService.setToken(response);
            this.router.navigate(['find-scholarships']);
          });
      })
      .catch((response) => {
        if (response.error?.errors) {
          this.errMsg = 'One or more validation errors occurred.';
          response.error?.errors?.forEach((form: any) => {
            this.setFormError(form.propertyName);
          });
        } else if (!response.success) {
          this.errMsg = response.error.message;
        }
        this.isError = true;
      });
    this.saving = false;
  }

  async googleRegister(socialUser: SocialUser) {
    this.saving = true;
    this.isError = false;

    await this.userService
      .googleRegister({ IdToken: socialUser.idToken })
      .then(async () => {
        await this.authService
          .googleLogin({
            IdToken: socialUser.idToken,
          })
          .then((response) => {
            this.tokenService.setToken(response);
            this.router.navigate(['find-scholarships']);
          });
      })
      .catch((response) => {
        if (response.error?.errors) {
          this.errMsg = 'One or more validation errors occurred.';
          response.error?.errors?.forEach((form: any) => {
            this.setFormError(form.propertyName);
          });
        } else if (!response.success) {
          this.errMsg = response.error.message;
        }
        this.isError = true;
      });
    this.saving = false;
  }

  async validateExistingEmail() {
    this.existingEmail = false;
    if (!!this.email && this.email.valid) {
      let request: ValidateUniqueEmailAddressRequestDTO = {
        email: this.email.value,
      };

      await this.userService
        .validateUniqueEmailAddress(request)
        .then((response) => {
          if (response.isUnique == false && !!this.email) {
            this.existingEmail = true;
            this.email.setErrors({ existingEmail: true });
          }
        })
        .catch((response) => {
          console.log(response);
        });
    }
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.registerForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default RegisterFormComponent;
