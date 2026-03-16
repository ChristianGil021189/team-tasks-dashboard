import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateTaskRequest, CreateTaskResponse } from '../../models/tasks/task.models';

@Injectable({
  providedIn: 'root'
})
export class TasksApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/tasks';

  createTask(request: CreateTaskRequest) {
    return this.http.post<CreateTaskResponse>(this.baseUrl, request);
  }
}