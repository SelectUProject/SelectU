import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Config } from './config';
import { TokenService } from './token.service';
import { ScholarshipApplicationDTO } from '../models/ScholarshipApplicationDTO';
import { ScholarshipSearchDTO } from '../models/ScholarshipSearchDTO';
import { ResponseDTO } from '../models/ResponseDTO';
import { ScholarshipCreateDTO } from '../models/ScholarshipCreateDTO';
import { ScholarshipUpdateDTO } from '../models/ScholarshipUpdateDTO';

@Injectable({
  providedIn: 'root',
})
export class ScholarshipService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async getScholarshipDetails(id: any) {
    return await firstValueFrom(
      this.http.get<ScholarshipUpdateDTO>(
        `${Config.api}/Scholarship/details`,
        id
      )
    );
  }
  async getActiveScholarships(scholarshipSearchDTO: ScholarshipSearchDTO) {
    return await firstValueFrom(
      this.http.post<ScholarshipUpdateDTO[]>(
        `${Config.api}/Scholarship/active-scholarships`,
        scholarshipSearchDTO
      )
    );
  }

  async getCreatedScholarship(scholarshipSearchDTO: ScholarshipSearchDTO) {
    return await firstValueFrom(
      this.http.post<ScholarshipUpdateDTO[]>(
        `${Config.api}/Scholarship/created-scholarships`,
        scholarshipSearchDTO
      )
    );
  }

  async createScholarship(scholarshipCreateDTO: ScholarshipCreateDTO) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(
        `${Config.api}/Scholarship/create-scholarship`,
        scholarshipCreateDTO
      )
    );
  }
}
