import { Component, OnInit } from '@angular/core';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import UserUpdateModalComponent from '../user-update-modal/user-update-modal.component';
import { UserService } from 'src/app/providers/user.service';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';

@Component({
  selector: 'app-user-table',
  templateUrl: './user-table.component.html',
  styleUrls: ['./user-table.component.scss'],
})
class UserTableComponent implements OnInit {
  userUpdateModalRef: MdbModalRef<UserUpdateModalComponent>;
  users: UserUpdateDTO[];
  success: boolean = false;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor(
    private userService: UserService,
    private modalService: MdbModalService
  ) {}

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers() {
    this.userService.getAllUsers().then((users) => {
      this.users = users;
    });
  }

  openModal(user: UserUpdateDTO) {
    this.success = false;
    this.userUpdateModalRef = this.modalService.open(UserUpdateModalComponent, {
      data: { user },
    });
    this.userUpdateModalRef.component.successEvent.subscribe(() => {
      this.success = true;
      this.getAllUsers();
      this.userUpdateModalRef.close();
    });
  }

  async kick(userId: string) {
    this.updating = true;
    this.success = false;

    const updateForm = {
      loginExpiry: new Date(),
    };

    await this.userService
      .updateLoginExpiry(userId, updateForm)
      .then(() => {
        this.success = true;
        this.getAllUsers();
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

export default UserTableComponent;
