import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { UserService } from 'src/app/providers/user.service';

@Component({
  selector: 'app-user-update-modal',
  templateUrl: './user-update-modal.component.html',
  styleUrls: ['./user-update-modal.component.scss'],
})
class UserUpdateModalComponent implements OnInit {
  @Output() successEvent = new EventEmitter();

  user: UserUpdateDTO;

  updateForm: FormGroup;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor(
    public UserUpdateModalRef: MdbModalRef<UserUpdateModalComponent>,
    private formBuilder: FormBuilder,
    private userService: UserService
  ) {}

  ngOnInit(): void {
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

    const updateForm = {
      loginExpiry: new Date(),
    };

    await this.userService
      .updateLoginExpiry(this.user.id, updateForm)
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

export default UserUpdateModalComponent;
