import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
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
  public scholarship: ScholarshipUpdateDTO;

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async getAllScholarships(scholarshipSearchDTO: ScholarshipSearchDTO) {
    return await firstValueFrom(
      this.http.post<ScholarshipUpdateDTO[]>(
        `${Config.api}/Scholarship`,
        scholarshipSearchDTO
      )
    );
  }

  async archiveScholarship(id: string) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(`${Config.api}/scholarship/archive/${id}`, {})
    );
  }

  async getScholarshipDetails(id: string) {
    return await firstValueFrom(
      this.http.get<ScholarshipUpdateDTO>(`${Config.api}/scholarship/${id}`)
    );
  }

  async getActiveScholarships(scholarshipSearchDTO: ScholarshipSearchDTO) {
    return await firstValueFrom(
      this.http.post<ScholarshipUpdateDTO[]>(
        `${Config.api}/Scholarship/active`,
        scholarshipSearchDTO
      )
    );
  }

  async getCreatedScholarship(scholarshipSearchDTO: ScholarshipSearchDTO) {
    return await firstValueFrom(
      this.http.post<ScholarshipUpdateDTO[]>(
        `${Config.api}/Scholarship/list/creator`,
        scholarshipSearchDTO
      )
    );
  }

  async createScholarship(scholarshipCreateDTO: ScholarshipCreateDTO) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(
        `${Config.api}/Scholarship/create`,
        scholarshipCreateDTO
      )
    );
  }
}
