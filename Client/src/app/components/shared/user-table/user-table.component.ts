import {
  Component,
  Directive,
  EventEmitter,
  Input,
  OnInit,
  Output,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import UserUpdateModalComponent from '../user-update-modal/user-update-modal.component';
import { UserService } from 'src/app/providers/user.service';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import {
  NgbdSortableHeader,
  SortEvent,
} from './ngbd-sortable-header/ngbd-sortable-header.component';
import { GenderEnum } from 'src/app/models/GenderEnum';

const compare = (
  v1: string | number | GenderEnum | Date,
  v2: string | number | GenderEnum | Date
) => (v1 > v2 ? -1 : v1 < v2 ? 1 : 0);

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

  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;

  constructor(
    private userService: UserService,
    private modalService: MdbModalService
  ) {}

  ngOnInit(): void {
    this.getAllUsers();
  }

  onSort({ column, direction }: SortEvent) {
    // resetting other headers
    this.headers.forEach((header) => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    // sorting users
    if (direction === '' || column === '') {
      this.getAllUsers();
    } else {
      this.users = [...this.users].sort((a, b) => {
        const res = compare(a[column], b[column]);
        return direction === 'desc' ? res : -res;
      });
    }
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
