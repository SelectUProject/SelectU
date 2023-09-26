import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Config } from './config';
import { TokenService } from './token.service';
import { ScholarshipApplicationDTO } from '../models/ScholarshipApplicationDTO';
import { ScholarshipApplicationSearchDTO } from '../models/ScholarshipApplicationSearchDTO';
import { ScholarshipApplicationCreateDTO } from '../models/ScholarshipApplicationCreateDTO';
import { ResponseDTO } from '../models/ResponseDTO';
import { ScholarshipApplicationUpdateDTO } from '../models/ScholarshipApplicationUpdateDTO';
@Injectable({
  providedIn: 'root',
})
export class ScholarshipApplicationService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async getScholarshipApplicationDetails(id: any) {
    return await firstValueFrom(
      this.http.get<ScholarshipApplicationUpdateDTO>(
        `${Config.api}/ScholarshipApplication/details`,
        id
      )
    );
  }

  async getMyScholarshipApplications(
    scholarshipApplicationSearchDTO: ScholarshipApplicationSearchDTO
  ) {
    return await firstValueFrom(
      this.http.post<ScholarshipApplicationUpdateDTO[]>(
        `${Config.api}/ScholarshipApplication/my-scholarship-applications`,
        scholarshipApplicationSearchDTO
      )
    );
  }

  async createScholarshipApplications(
    scholarshipApplicationCreateDTO: ScholarshipApplicationCreateDTO
  ) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(
        `${Config.api}/ScholarshipApplication/create-scholarship-application`,
        scholarshipApplicationCreateDTO
      )
    );
  }
}
