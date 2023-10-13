import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ResponseDTO } from '../models/ResponseDTO';
import { UserUpdateDTO } from '../models/UserUpdateDTO';
import { Config } from './config';
import { TokenService } from './token.service';
import { UpdateUserRolesDTO } from '../models/UpdateUserRolesDTO';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async updateUserDetails(user: UserUpdateDTO) {
    return await firstValueFrom(
      this.http.patch<ResponseDTO>(`${Config.api}/admin/details/update`, user)
    );
  }

  async deleteUser(userId: string) {
    return await firstValueFrom(
      this.http.delete<ResponseDTO>(`${Config.api}/admin/delete/${userId}`)
    );
  }

  async updateRoles(updateDTO: UpdateUserRolesDTO) {
    return await firstValueFrom(
      this.http.patch<ResponseDTO>(
        `${Config.api}/admin/roles/update`,
        updateDTO
      )
    );
  }

  async getRoles(userId: string) {
    return await firstValueFrom(
      this.http.get<string[]>(`${Config.api}/admin/roles/${userId})`)
    );
  }
}
