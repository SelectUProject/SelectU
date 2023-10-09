import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { RegistrationResponseDTO } from '../models/RegistrationResponseDTO';
import { UserRegisterDTO } from '../models/UserRegisterDTO';
import { Config } from './config';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async register(registerDTO: UserRegisterDTO) {
    return await firstValueFrom(
      this.http.post<RegistrationResponseDTO>(
        `${Config.api}/user/register`,
        registerDTO
      )
    );
  }
}
