import { Component, EventEmitter, Output } from '@angular/core';
import { AdminService } from 'src/app/providers/admin.service';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';
import { UserService } from 'src/app/providers/user.service';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';



@Component({
  selector: 'app-admin-confirm-delete-modal',
  templateUrl: './admin-confirm-delete-modal.component.html',
  styleUrls: ['./admin-confirm-delete-modal.component.scss']
})
export class AdminConfirmDeleteModalComponent{
  @Output() successEvent = new EventEmitter();
  success: boolean = false;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;
  user: UserUpdateDTO;
  users: UserUpdateDTO[];

  constructor(
    public UserDeleteModalRef: MdbModalRef<AdminConfirmDeleteModalComponent>,
    private adminService: AdminService,

  ) {}

  async delete(userId: string) {
    this.updating = true;
    this.success = false;

    await this.adminService
      .deleteUser(userId)
      .then(() => {
        this.successEvent.emit();
      })
      .catch((response) => {
        if (response.error?.errors) {
          this.errMsg = 'One or more validation errors occurred.';
          console.log(response.error.errors);
        } else if (!response.success) {
          console.log(response.error.message);
        }
      });
    this.updating = false;
}
}
