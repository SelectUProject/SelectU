import { Component, OnInit } from '@angular/core';
import { TempUserDTO } from 'src/app/models/TempUserDTO';
import { TempUserService } from 'src/app/providers/tempUser.service';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import TempUserUpdateModalComponent from '../temp-user-update-modal/temp-user-update-modal.component';

@Component({
  selector: 'app-temp-user-table',
  templateUrl: './temp-user-table.component.html',
  styleUrls: ['./temp-user-table.component.scss'],
})
class TempUserTableComponent implements OnInit {
  tempUserUpdateModalRef: MdbModalRef<TempUserUpdateModalComponent>;
  tempUsers: TempUserDTO[];
  success: boolean = false;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  constructor(
    private tempUserService: TempUserService,
    private modalService: MdbModalService
  ) {}

  ngOnInit(): void {
    this.getTempUsers();
  }

  getTempUsers() {
    this.tempUserService.getTempUsers().then((tempUsers) => {
      this.tempUsers = tempUsers;
    });
  }

  openModal(user: TempUserDTO) {
    this.success = false;
    this.tempUserUpdateModalRef = this.modalService.open(
      TempUserUpdateModalComponent,
      { data: { user } }
    );
    this.tempUserUpdateModalRef.component.successEvent.subscribe(() => {
      this.success = true;
      this.getTempUsers();
      this.tempUserUpdateModalRef.close();
    });
  }

  async kick(userId: string) {
    this.updating = true;
    this.success = false;

    const updateForm = {
      id: userId,
      loginExpiry: new Date(),
    };

    await this.tempUserService
      .updateTempUserExpiry(updateForm)
      .then(() => {
        this.success = true;
        this.getTempUsers();
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

export default TempUserTableComponent;
