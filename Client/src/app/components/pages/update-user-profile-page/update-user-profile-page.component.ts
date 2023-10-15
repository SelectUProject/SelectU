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
import { DatePipe } from '@angular/common'
@Component({
  selector: 'app-update-user-profile-page',
  templateUrl: './update-user-profile-page.component.html',
  styleUrls: ['./update-user-profile-page.component.scss'],
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
        this.setupForm();
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

  get dateOfBirth() {
    return this.updateAccountForm.get('dateOfBirth');
  }

  get gender() {
    return this.updateAccountForm.get('gender');
  }

  get phoneNumber() {
    return this.updateAccountForm.get('phoneNumber');
  }

  get address() {
    return this.updateAccountForm.get('address');
  }

  get suburb() {
    return this.updateAccountForm.get('suburb');
  }

  get postcode() {
    return this.updateAccountForm.get('postcode');
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
    private socialAuthService: SocialAuthService,
    private datepipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.getUserDetails();

    this.setupSocialAuthService();
  }

  setupSocialAuthService() {
    this.socialAuthService.authState.subscribe((user) => {
      this.socialUser = user;
      console.log(this.socialUser);
    });
  }

  setupForm() {
    this.updateAccountForm = this.formBuilder.group({
      email: [
        this.userDetails.email,
        Validators.compose([
          Validators.required,
          Validators.email
        ]),
      ],
      firstName: [
        this.userDetails.firstName,
        Validators.compose([
          Validators.required,
          Validators.pattern('^[a-zA-Z ]+$'),
        ]),
      ],
      lastName: [
        this.userDetails.lastName,
        Validators.compose([
          Validators.required,
          Validators.pattern('^[a-zA-Z ]+$'),
        ]),
      ],
      dateOfBirth: [this.datepipe.transform(this.userDetails.dateOfBirth, 'dd/MM/yyyy'), Validators.required],
      gender: [this.userDetails.gender, Validators.required],
      phoneNumber: [
        this.userDetails.phoneNumber,
        Validators.compose([
          Validators.required,
          Validators.pattern('^[0-9]+$'),
        ]),
      ],
      address: [this.userDetails.address, Validators.required],
      suburb: [this.userDetails.suburb, Validators.required],
      postcode: [this.userDetails.postcode, Validators.required],
      state: [this.userDetails.state, Validators.required],
      country: [this.userDetails.country, Validators.required],
    });
  }

  async updateProfile() {
    this.saving = true;
    this.isError = false;
    let updateAccountForm = <UserUpdateDTO>this.updateAccountForm.value;
    updateAccountForm.id = this.userDetails.id;
    await this.userService
      .updateUserDetails(updateAccountForm)
      .then(() => {
        console.log('Successfully Updated');
        this.router.navigate(['account']);
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

  // async validateExistingEmail() {
  //   this.existingEmail = false;
  //   if (!!this.email && this.email.valid) {
  //     let request: ValidateUniqueEmailAddressRequestDTO = {
  //       email: this.email.value,
  //     };

  //     await this.userService
  //       .validateUniqueEmailAddress(request)
  //       .then((response) => {
  //         if (response.isUnique == false && !!this.email) {
  //           this.existingEmail = true;
  //           this.email.setErrors({ existingEmail: true });
  //         }
  //       })
  //       .catch((response) => {
  //         console.log(response);
  //       });
  //   }
  // }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.updateAccountForm.controls[propertyName]?.setErrors({
      required: true,
    });
  }
}
