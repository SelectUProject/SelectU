import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { TempUserDTO } from 'src/app/models/TempUserDTO';
import { TempUserUpdateDTO } from 'src/app/models/TempUserUpdateDTO';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { TempUserService } from 'src/app/providers/tempUser.service';

@Component({
  selector: 'app-temp-user-update-modal',
  templateUrl: './temp-user-update-modal.component.html',
  styleUrls: ['./temp-user-update-modal.component.scss'],
})
class TempUserUpdateModalComponent implements OnInit {
  @Output() successEvent = new EventEmitter();

  user: TempUserDTO;

  updateForm: FormGroup;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor(
    public tempUserUpdateModalRef: MdbModalRef<TempUserUpdateModalComponent>,
    private formBuilder: FormBuilder,
    private tempUserService: TempUserService
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

    let updateForm = <TempUserUpdateDTO>this.updateForm.value;
    updateForm.id = this.user.id;

    await this.tempUserService
      .updateTempUserExpiry(updateForm)
      .then(() => {
        this.successEvent.emit();
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
      });
    this.updating = false;
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.updateForm.controls[propertyName]?.setErrors({ required: true });
  }
}

export default TempUserUpdateModalComponent;
