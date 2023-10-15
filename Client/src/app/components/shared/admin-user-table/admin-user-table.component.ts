import {
  Component,
  OnInit,
  Output,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import { UserService } from 'src/app/providers/user.service';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import {
  NgbdSortableHeader,
  SortEvent,
} from './ngbd-sortable-header/ngbd-sortable-header.component';
import { GenderEnum } from 'src/app/models/GenderEnum';
import { AdminService } from 'src/app/providers/admin.service';
import { AdminUserUpdateModalComponent } from '../admin-user-update-modal/admin-user-update-modal.component';
import { AdminConfirmDeleteModalComponent } from '../admin-confirm-delete-modal/admin-confirm-delete-modal.component';

const compare = (
  v1: string | number | GenderEnum | Date,
  v2: string | number | GenderEnum | Date
) => (v1 > v2 ? -1 : v1 < v2 ? 1 : 0);

@Component({
  selector: 'app-admin-user-table',
  templateUrl: './admin-user-table.component.html',
  styleUrls: ['./admin-user-table.component.scss'],
})
class AdminUserTableComponent implements OnInit {
  userUpdateModalRef: MdbModalRef<AdminUserUpdateModalComponent>;
  userDeleteModalRef: MdbModalRef<AdminConfirmDeleteModalComponent>;
  users: UserUpdateDTO[];
  success: boolean = false;
  isError: boolean = false;
  errMsg: string = 'An error has occurred!';
  updating: boolean = false;

  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;

  constructor(
    private userService: UserService,
    private modalService: MdbModalService,
    private adminService: AdminService
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

  openModalUpdate(user: UserUpdateDTO) {
    this.success = false;
    this.userUpdateModalRef = this.modalService.open(AdminUserUpdateModalComponent, {
      data: { user },
    });
    this.userUpdateModalRef.component.successEvent.subscribe(() => {
      this.success = true;
      this.getAllUsers();
      this.userUpdateModalRef.close();
    });
  }
  
  openModalDelete(user: UserUpdateDTO) {
    this.success = false;
    this.userDeleteModalRef = this.modalService.open(AdminConfirmDeleteModalComponent, {
      data: { user },
    });
    this.userDeleteModalRef.component.successEvent.subscribe(() => {
      this.success = true;
      this.getAllUsers();
      this.userDeleteModalRef.close();
    });
  }

}

export default AdminUserTableComponent;
