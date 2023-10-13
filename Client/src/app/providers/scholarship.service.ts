import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  async getScholarshipDetails(id: any) {
    return await firstValueFrom(
      this.http.get<ScholarshipUpdateDTO>(
        `${Config.api}/Scholarship/details?id=${id}`
      )
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
      this.http.post<ScholarshipUpdateDTO>(
        `${Config.api}/Scholarship/create`,
        scholarshipCreateDTO
      )
    );
  }

  async updateScholarship(scholarshipCreateDTO: ScholarshipCreateDTO) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(
        `${Config.api}/Scholarship/update`,
        scholarshipCreateDTO
      )
    );
  }

  async deleteScholarship(id: any) {
    return await firstValueFrom(
      this.http.delete<ResponseDTO>(
        `${Config.api}/Scholarship/delete/${id}`,
      )
    );
  }

  async uploadScholarshipImg(id: any, formData: FormData) {
    // const headerDict = {
    //   'Content-Type': 'multipart/form-data',
    //   'Accept': 'application/json',
    //   'Access-Control-Allow-Headers': 'Content-Type',
    // }

    // const requestOptions = {
    //   headers: new HttpHeaders(headerDict),
    // };

    // const headers = {
    //   'Content-Type': 'multipart/form-data',
    // };

    // formData.append('fake', 'fake');

    // const httpOptions = {
    //   headers: new HttpHeaders({
    //     'Content-Type': 'multipart/form-data',
    //   }),
    // };

    return new Promise((resolve, reject) => {
      const xhr = new XMLHttpRequest();
      xhr.open('POST', `${Config.api}/Scholarship/photo/upload/${id}`, true);
      xhr.setRequestHeader('Content-Type', 'multipart/form-data');

      xhr.onload = () => {
        if (xhr.status === 200) {
          resolve(xhr.response);
        } else {
          reject(xhr.response);
        }
      };

      xhr.send(formData);
    });
  }
}
