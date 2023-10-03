import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TempUserInviteDTO } from 'src/app/models/TempUserInviteDTO';
import { ValidateUniqueEmailAddressRequestDTO } from 'src/app/models/ValidateUniqueEmailAddressDTO';
import { Config } from 'src/app/providers/config';
import { TempUserService } from 'src/app/providers/tempUser.service';

@Component({
  selector: 'app-temp-user-invite-form',
  templateUrl: './temp-user-invite-form.component.html',
  styleUrls: ['./temp-user-invite-form.component.scss'],
})
class TempUserInviteFormComponent implements OnInit {
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
    private tempUserService: TempUserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  setupForm() {
    this.inviteForm = this.formBuilder.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
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
      loginExpiry: ['', Validators.required],
    });
  }

  async validateExistingEmail() {
    this.existingEmail = false;

    if (!!this.email && this.email.valid) {
      let request: ValidateUniqueEmailAddressRequestDTO = {
        email: this.email.value,
      };

      await this.tempUserService
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

    let inviteForm = <TempUserInviteDTO>this.inviteForm.value;

    await this.tempUserService
      .inviteTempUser(inviteForm)
      .then(() => {
        this.success = true;
        this.router.navigate(['/view-temp-users']);
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

export default TempUserInviteFormComponent;
