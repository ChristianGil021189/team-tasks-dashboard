import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  DeveloperDelayRiskDto,
  DeveloperWorkloadDto,
  ProjectHealthDto
} from '../../models/dashboard/dashboard.models';

@Injectable({
  providedIn: 'root'
})
export class DashboardApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/dashboard';

  getDeveloperWorkload(): Observable<DeveloperWorkloadDto[]> {
    return this.http.get<DeveloperWorkloadDto[]>(`${this.baseUrl}/developer-workload`);
  }

  getProjectHealth(): Observable<ProjectHealthDto[]> {
    return this.http.get<ProjectHealthDto[]>(`${this.baseUrl}/project-health`);
  }

  getDeveloperDelayRisk(): Observable<DeveloperDelayRiskDto[]> {
    return this.http.get<DeveloperDelayRiskDto[]>(`${this.baseUrl}/developer-delay-risk`);
  }
}