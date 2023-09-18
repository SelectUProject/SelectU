import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Config } from './config';
import { TokenService } from './token.service';
import { ScholarshipApplicationDTO } from '../models/ScholarshipApplicationDTO';
import { ScholarshipApplicationSearchDTO } from '../models/ScholarshipApplicationSearchDTO';
@Injectable({
  providedIn: 'root',
})
export class ScholarshipApplicationService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async getScholarshipApplicationDetails(id: any) {
    return await firstValueFrom(
      this.http.get<ScholarshipApplicationDTO>(
        `${Config.api}/ScholarshipApplication/details`,
        id
      )
    );
  }

  async getMyScholarshipApplications(
    scholarshipApplicationSearchDTO: ScholarshipApplicationSearchDTO
  ) {
    return await firstValueFrom(
      this.http.patch<ScholarshipApplicationDTO[]>(
        `${Config.api}/ScholarshipApplication/my-scholarship-applications`,
        scholarshipApplicationSearchDTO
      )
    );
  }
}
