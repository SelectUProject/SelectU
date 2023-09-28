import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import {
  ValidateUniqueEmailAddressRequestDTO,
  ValidateUniqueEmailAddressResponseDTO,
} from '../models/ValidateUniqueEmailAddressDTO';
import { Config } from './config';
import { TempUserInviteDTO } from '../models/TempUserInviteDTO';
import { TempUserDTO } from '../models/TempUserDTO';

@Injectable({
  providedIn: 'root',
})
export class TempUserService {
  constructor(private http: HttpClient) {}

  async validateUniqueEmailAddress(
    request: ValidateUniqueEmailAddressRequestDTO
  ) {
    return await firstValueFrom(
      this.http.post<ValidateUniqueEmailAddressResponseDTO>(
        `${Config.api}/tempUser/validate`,
        request
      )
    );
  }

  async inviteTempUser(request: TempUserInviteDTO) {
    return await firstValueFrom(
      this.http.post<ValidateUniqueEmailAddressResponseDTO>(
        `${Config.api}/tempUser/invite`,
        request
      )
    );
  }

  async getTempUsers() {
    return await firstValueFrom(
      this.http.get<TempUserDTO[]>(`${Config.api}/tempUser/list`)
    );
  }
}
