import { Component, OnInit } from '@angular/core';
import { TempUserDTO } from 'src/app/models/TempUserDTO';
import { TempUserService } from 'src/app/providers/tempUser.service';

@Component({
  selector: 'app-temp-user-table',
  templateUrl: './temp-user-table.component.html',
  styleUrls: ['./temp-user-table.component.scss'],
})
class TempUserTableComponent implements OnInit {
  tempUsers: TempUserDTO[];

  constructor(private tempUserService: TempUserService) {}

  ngOnInit(): void {
    this.getTempUsers();
  }

  getTempUsers() {
    this.tempUserService.getTempUsers().then((tempUsers) => {
      this.tempUsers = tempUsers;
    });
  }
}

export default TempUserTableComponent;
