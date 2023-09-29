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
    this.tempUserUpdateModalRef = this.modalService.open(
      TempUserUpdateModalComponent,
      { data: { user } }
    );
  }
}

export default TempUserTableComponent;
