import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TempUserInviteDTO } from 'src/app/models/TempUserInviteDTO';
import { ValidateUniqueEmailAddressRequestDTO } from 'src/app/models/ValidateUniqueEmailAddressDTO';
import { AuthService } from 'src/app/providers/auth.service';
import { TempUserService } from 'src/app/providers/tempUser.service';

@Component({
  selector: 'app-temp-user-invite-form',
  templateUrl: './temp-user-invite-form.component.html',
  styleUrls: ['./temp-user-invite-form.component.scss'],
})
class TempUserInviteFormComponent {
  inviteForm: FormGroup;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  existingEmail: boolean = false;
  saving: boolean = false;

  get email() {
    return this.inviteForm.get('email');
  }

  get firstName() {
    return this.inviteForm.get('firstName');
  }

  get lastName() {
    return this.inviteForm.get('lastName');
  }

  get expiry() {
    return this.inviteForm.get('expiry');
  }

  constructor(
    private formBuilder: FormBuilder,
    private tempUserService: TempUserService
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
      expiry: ['', Validators.required],
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
    this.saving = true;
    this.isError = false;
    console.log(this.inviteForm.value);

    let inviteForm = <TempUserInviteDTO>this.inviteForm.value;

    await this.tempUserService
      .inviteTempUser(inviteForm)
      .then(() => {
        console.log('Successful Invitation');
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

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.inviteForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default TempUserInviteFormComponent;
