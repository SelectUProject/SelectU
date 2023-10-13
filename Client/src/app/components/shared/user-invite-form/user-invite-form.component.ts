import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserInviteDTO } from 'src/app/models/UserInviteDTO';
import { ValidateUniqueEmailAddressRequestDTO } from 'src/app/models/ValidateUniqueEmailAddressDTO';
import { Config } from 'src/app/providers/config';
import { UserService } from 'src/app/providers/user.service';
import { USER, STAFF, REVIEWER } from 'src/app/constants/userRoles';

@Component({
  selector: 'app-user-invite-form',
  templateUrl: './user-invite-form.component.html',
  styleUrls: ['./user-invite-form.component.scss'],
})
class UserInviteFormComponent implements OnInit {
  roles: string[] = [USER, STAFF, REVIEWER];
  inviteForm: FormGroup;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  existingEmail: boolean = false;
  inviting: boolean = false;
  success: boolean = false;
  timezone = Config.dateTimezone;
  format = Config.dateFormat;

  get email() {
    return this.inviteForm.get('email');
  }

  get role() {
    return this.inviteForm.get('role');
  }

  get firstName() {
    return this.inviteForm.get('firstName');
  }

  get lastName() {
    return this.inviteForm.get('lastName');
  }

  get loginExpiry() {
    return this.inviteForm.get('loginExpiry');
  }

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  setupForm() {
    this.inviteForm = this.formBuilder.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
      role: ['', Validators.required],
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
      loginExpiry: [''],
    });
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

  async invite() {
    this.inviting = true;
    this.isError = false;

    let inviteForm = <UserInviteDTO>this.inviteForm.value;
    inviteForm.loginExpiry = new Date(inviteForm.loginExpiry);

    await this.userService
      .inviteUser(inviteForm)
      .then(() => {
        this.success = true;
        this.router.navigate(['/view-users']);
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
    this.inviting = false;
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.inviteForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default UserInviteFormComponent;
