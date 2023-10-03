import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ChangePasswordDTO } from '../models/ChangePasswordDTO';
import { ForgotPasswordDto } from '../models/ForgotPasswordDto';
import { RegistrationResponseDTO } from '../models/RegistrationResponseDTO';
import { ResetPasswordDto } from '../models/ResetPasswordDto';
import { ResponseDTO } from '../models/ResponseDTO';
import { UserRegisterDTO } from '../models/UserRegisterDTO';
import { UserUpdateDTO } from '../models/UserUpdateDTO';
import {
  ValidateUniqueEmailAddressRequestDTO,
  ValidateUniqueEmailAddressResponseDTO,
} from '../models/ValidateUniqueEmailAddressDTO';
import { Config } from './config';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async validateUniqueEmailAddress(
    request: ValidateUniqueEmailAddressRequestDTO
  ) {
    return await firstValueFrom(
      this.http.post<ValidateUniqueEmailAddressResponseDTO>(
        `${Config.api}/user/validate`,
        request
      )
    );
  }

  async register(registerDTO: UserRegisterDTO) {
    return await firstValueFrom(
      this.http.post<RegistrationResponseDTO>(
        `${Config.api}/user/register`,
        registerDTO
      )
    );
  }

  async getUserDetails() {
    return await firstValueFrom(
      this.http.get<UserUpdateDTO>(`${Config.api}/User/details`)
    );
  }

  async updateUserDetails(userDetails: UserUpdateDTO) {
    return await firstValueFrom(
      this.http.patch<ResponseDTO>(
        `${Config.api}/user/details/update`,
        userDetails
      )
    );
  }

  async changePassword(passwordDTO: ChangePasswordDTO) {
    return await firstValueFrom(
      this.http.patch<ResponseDTO>(
        `${Config.api}/user/change-password`,
        passwordDTO
      )
    );
  }

  async forgotPassword(forgotPasswordDto: ForgotPasswordDto) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(
        `${Config.api}/user/forgot-password`,
        forgotPasswordDto
      )
    );
  }

  async resetPassword(ResetPasswordDto: ResetPasswordDto) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(
        `${Config.api}/user/reset-password`,
        ResetPasswordDto
      )
    );
  }

  async adminUpdateUserDetails(user: UserUpdateDTO) {
    return await firstValueFrom(
      this.http.patch<ResponseDTO>(
        `${Config.api}/user/admin/details/update`,
        user
      )
    );
  }
}
