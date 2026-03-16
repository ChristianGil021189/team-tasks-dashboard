import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { DeveloperDto } from '../../models/developers/developer.models';

@Injectable({
  providedIn: 'root'
})
export class DevelopersApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/developers';

  getDevelopers(): Observable<DeveloperDto[]> {
    return this.http.get<DeveloperDto[]>(this.baseUrl);
  }
}