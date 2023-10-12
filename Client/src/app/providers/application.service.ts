import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  async getScholarshipApplications(
    scholarshipId: string,
    scholarshipApplicationSearchDTO: ScholarshipApplicationSearchDTO
  ) {
    return await firstValueFrom(
      this.http.post<ScholarshipApplicationUpdateDTO[]>(
        `${Config.api}/scholarshipApplication/${scholarshipId}`,
        scholarshipApplicationSearchDTO
      )
    );
  }

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

  async uploadFile(file: FormData) {
    const headers = new HttpHeaders().set('Accept', 'application/json');
    headers.set('Content-Type', 'multipart/form-data');

    return await firstValueFrom(
      this.http.post<any>(
        `${Config.api}/ScholarshipApplication/file-upload`,
        file,
        {
          headers,
        }
      )
    );
  }

  async fileDownload(fileUri: string) {
    return await firstValueFrom(
      this.http.post<any>(
        `${Config.api}/ScholarshipApplication/file-download?fileUri=${fileUri}`,
        {}
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

  async selectApplication(
    scholarshipApplicationUpdateDTO: ScholarshipApplicationUpdateDTO
  ) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(
        `${Config.api}/ScholarshipApplication/select-application`,
        scholarshipApplicationUpdateDTO
      )
    );
  }
}
