import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { AdminService } from 'src/app/providers/admin.service';
import { UserService } from 'src/app/providers/user.service';
import { USER, STAFF, REVIEWER, ADMIN } from 'src/app/constants/userRoles';
import { UpdateUserRolesDTO } from 'src/app/models/UpdateUserRolesDTO';
@Component({
  selector: 'app-admin-user-update-modal',
  templateUrl: './admin-user-update-modal.component.html',
  styleUrls: ['./admin-user-update-modal.component.scss'],
})
export class AdminUserUpdateModalComponent implements OnInit {
  @Output() successEvent = new EventEmitter();

  user: UserUpdateDTO;
  updateForm: FormGroup;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;
  userRole: UpdateUserRolesDTO;
  roles: string[] = [USER, STAFF, REVIEWER, ADMIN];

  get role() {
    return this.updateForm.get('role');
  }

  constructor(
    public UserUpdateModalRef: MdbModalRef<AdminUserUpdateModalComponent>,
    private formBuilder: FormBuilder,
    private userService: UserService,
    private adminService: AdminService
  ) {}

  ngOnInit(): void {
    this.setupForm();
  }

  setupForm() {
    this.updateForm = this.formBuilder.group({
      email: [
        { value: this.user.email, disabled: false },
        Validators.compose([Validators.required, Validators.email]),
      ],
      firstName: [
        { value: this.user.firstName, disabled: false },
        Validators.compose([
          Validators.required,
          Validators.pattern('^[a-zA-Z ]+$'),
        ]),
      ],
      lastName: [
        { value: this.user.lastName, disabled: false },
        Validators.compose([
          Validators.required,
          Validators.pattern('^[a-zA-Z ]+$'),
        ]),
      ],
      role: [{ value: this.user.role, disabled: false }, Validators.required],
    });
  }

  async update() {
    this.updating = true;

    let updateForm = <UserUpdateDTO>this.updateForm.value;
    this.user.firstName = updateForm.firstName;
    this.user.lastName = updateForm.lastName;
    this.user.email = updateForm.email;

    await this.adminService
      .updateUserDetails(this.user)
      .then(() => {
        // this.successEvent.emit();
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

    let userRole = {
      userId: this.user.id,
      removeRoles: [this.user.role],
      AddRoles: [updateForm.role],
    };
    await this.adminService
      .updateRoles(userRole)
      .then(() => {
        // this.successEvent.emit();
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

    this.successEvent.emit(); 
    this.updating = false;
  }

  setFormError(propertyName: string) {
    propertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
    this.updateForm.controls[propertyName]?.setErrors({ required: true });
  }
}
