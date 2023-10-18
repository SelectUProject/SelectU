import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Config } from './config';
import { TokenService } from './token.service';
import { ReviewDTO } from '../models/ReviewDTO';
import { ResponseDTO } from '../models/ResponseDTO';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  async review(reviewDTO: ReviewDTO) {
    return await firstValueFrom(
      this.http.post<ResponseDTO>(`${Config.api}/review`, reviewDTO)
    );
  }
  async updateReview(reviewDTO: ReviewDTO) {
    return await firstValueFrom(
      this.http.patch<ResponseDTO>(`${Config.api}/review`, reviewDTO)
    );
  }

  async getAverageRating(applicationId: string) {
    return await firstValueFrom(
      this.http.get<number>(
        `${Config.api}/review/average-rating/${applicationId}`
      )
    );
  }

  async getMyReview(applicationId: string) {
    return await firstValueFrom(
      this.http.get<ReviewDTO>(
        `${Config.api}/review/application/${applicationId}/mine`
      )
    );
  }

  async deleteReview(reviewId: string) {
    return await firstValueFrom(
      this.http.delete<ResponseDTO>(`${Config.api}/review/${reviewId}`)
    );
  }
}
