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
import { UserRegisterDTO } from 'src/app/models/UserRegisterDTO';
import { ValidateUniqueEmailAddressRequestDTO } from 'src/app/models/ValidateUniqueEmailAddressDTO';
import { AuthService } from 'src/app/providers/auth.service';
import { TokenService } from 'src/app/providers/token.service';
import { UserService } from 'src/app/providers/user.service';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';

@Component({
  selector: 'app-update-user-profile-page',
  templateUrl: './update-user-profile-page.component.html',
  styleUrls: ['./update-user-profile-page.component.scss']
})
export class UpdateUserProfilePageComponent {
  genders: NumberLookupDTO[] = GENDERS_LIST;
  states: StringLookupDTO[] = STATES_LIST;
  updateAccountForm: FormGroup;
  saving: boolean = false;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  existingEmail: boolean = false;
  todayDate: Date = new Date();
  registered: boolean = false;
  socialUser: SocialUser;

  // Fill form with existing account information: 
  userDetails: UserUpdateDTO;

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




  get email() {
    return this.updateAccountForm.get('email');
  }

  get firstName() {
    return this.updateAccountForm.get('firstName');
  }

  get lastName() {
    return this.updateAccountForm.get('lastName');
  }

  // get password() {
  //   return this.updateAccountForm.get('password');
  // }

  // get confirmPassword() {
  //   return this.updateAccountForm.get('confirmPassword');
  // }

  get dateOfBirth() {
    return this.updateAccountForm.get('dateOfBirth');
  }

  get gender() {
    return this.updateAccountForm.get('gender');
  }

  get phoneNumber() {
    return this.updateAccountForm.get('phoneNumber');
  }

  get state() {
    return this.updateAccountForm.get('state');
  }

  get country() {
    return this.updateAccountForm.get('country');
  }

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private socialAuthService: SocialAuthService
  ) {}

  ngOnInit(): void {
    // fill existing account information
    this.getUserDetails();

    this.setupForm(this.userDetails);
    this.setupSocialAuthService();
  }

  setupSocialAuthService() {
    this.socialAuthService.authState.subscribe((user) => {
      this.socialUser = user;
      console.log(this.socialUser);
    });
  }

  setupForm(userDetails: UserUpdateDTO) {
    userDetails = userDetails; 
    this.updateAccountForm = this.formBuilder.group(
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

async updateProfile(){

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
    this.updateAccountForm.controls[propertyName]?.setErrors({ required: true });
  }



}
