import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthToken } from '../models/AuthToken';
import { LoginDTO } from '../models/LoginDTO';
import { ResponseDTO } from '../models/ResponseDTO';
import { Config } from './config';
import { TokenService } from './token.service';
import { GoogleAuthDTO } from '../models/GoogleAuthDTO';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async login(loginDto: LoginDTO) {
    return await firstValueFrom(
      this.http.post<AuthToken>(`${Config.api}/authenticate/login`, loginDto)
    );
  }

  async googleLogin(authDTO: GoogleAuthDTO) {
    return await firstValueFrom(
      this.http.post<AuthToken>(
        `${Config.api}/authenticate/google-login`,
        authDTO
      )
    );
  }

  async validateToken() {
    return await firstValueFrom(
      this.http.get<ResponseDTO>(`${Config.api}/authenticate/validate-token`)
    );
  }
}
