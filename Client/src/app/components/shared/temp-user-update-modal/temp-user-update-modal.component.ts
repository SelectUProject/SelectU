import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { TempUserDTO } from 'src/app/models/TempUserDTO';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { UserService } from 'src/app/providers/user.service';

@Component({
  selector: 'app-temp-user-update-modal',
  templateUrl: './temp-user-update-modal.component.html',
  styleUrls: ['./temp-user-update-modal.component.scss'],
})
class TempUserUpdateModalComponent implements OnInit {
  user: TempUserDTO;

  updateForm: FormGroup;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;
  success: boolean = false;

  constructor(
    public tempUserUpdateModalRef: MdbModalRef<TempUserUpdateModalComponent>,
    private formBuilder: FormBuilder,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    console.log(this.user);

    this.setupForm();
  }

  setupForm() {
    this.updateForm = this.formBuilder.group({
      email: [
        { value: this.user.email, disabled: true },
        Validators.compose([Validators.required, Validators.email]),
      ],
      firstName: [
        { value: this.user.firstName, disabled: true },
        Validators.compose([
          Validators.required,
          Validators.pattern('^[a-zA-Z ]+$'),
        ]),
      ],
      lastName: [
        { value: this.user.lastName, disabled: true },
        Validators.compose([
          Validators.required,
          Validators.pattern('^[a-zA-Z ]+$'),
        ]),
      ],
      loginExpiry: [new Date(this.user.loginExpiry), Validators.required],
    });
  }

  async update() {
    this.updating = true;
    this.isError = false;

    let updateForm = <UserUpdateDTO>this.updateForm.value;
    updateForm.id = this.user.id;

    await this.userService
      .adminUpdateUserDetails(updateForm)
      .then(() => {
        this.success = true;
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
    this.updating = false;
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.updateForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default TempUserUpdateModalComponent;
