import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
  PagedResponse,
  ProjectSummaryDto,
  ProjectTaskDto,
  ProjectTasksQuery
} from '../../models/projects/projects.models';

@Injectable({
  providedIn: 'root'
})
export class ProjectsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/projects';

  getProjects(): Observable<ProjectSummaryDto[]> {
    return this.http.get<ProjectSummaryDto[]>(this.baseUrl);
  }

  getProjectTasks(
    projectId: number,
    query: ProjectTasksQuery
  ): Observable<PagedResponse<ProjectTaskDto>> {
    let params = new HttpParams()
      .set('page', query.page)
      .set('pageSize', query.pageSize);

    if (query.status !== null && query.status !== undefined) {
      params = params.set('status', query.status);
    }

    if (query.assigneeId !== null && query.assigneeId !== undefined) {
      params = params.set('assigneeId', query.assigneeId);
    }

    return this.http.get<PagedResponse<ProjectTaskDto>>(
      `${this.baseUrl}/${projectId}/tasks`,
      { params }
    );
  }
}