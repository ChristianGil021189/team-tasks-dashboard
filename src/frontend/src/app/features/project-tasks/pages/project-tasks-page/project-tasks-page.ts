import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BehaviorSubject, combineLatest, map, shareReplay, switchMap } from 'rxjs';

import { DevelopersApiService } from '../../../../data-access/developers/developers-api.service';
import { ProjectsApiService } from '../../../../data-access/projects/projects-api.service';
import { DeveloperDto } from '../../../../models/developers/developer.models';
import { ProjectSummaryDto, ProjectTaskDto } from '../../../../models/projects/projects.models';
import { TaskItemStatus, TaskPriority } from '../../../../models/shared/app.enums';
import { EnumLabelPipe } from '../../../../shared/pipes/enum-label-pipe';

@Component({
  selector: 'app-project-tasks-page',
  standalone: true,
  imports: [CommonModule, RouterLink, EnumLabelPipe],
  templateUrl: './project-tasks-page.html',
  styleUrl: './project-tasks-page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProjectTasksPage {
  private readonly route = inject(ActivatedRoute);
  private readonly projectsApiService = inject(ProjectsApiService);
  private readonly developersApiService = inject(DevelopersApiService);

  protected readonly taskItemStatus = TaskItemStatus;
  protected readonly taskPriority = TaskPriority;

  protected readonly taskStatuses = [
    { label: 'All', value: null },
    { label: 'To Do', value: TaskItemStatus.ToDo },
    { label: 'In Progress', value: TaskItemStatus.InProgress },
    { label: 'Blocked', value: TaskItemStatus.Blocked },
    { label: 'Completed', value: TaskItemStatus.Completed }
  ];

  private readonly selectedStatusSubject = new BehaviorSubject<TaskItemStatus | null>(null);
  private readonly selectedAssigneeIdSubject = new BehaviorSubject<number | null>(null);
  private readonly currentPageSubject = new BehaviorSubject<number>(1);
  private readonly pageSizeSubject = new BehaviorSubject<number>(10);
  private readonly selectedTaskSubject = new BehaviorSubject<ProjectTaskDto | null>(null);

  protected readonly selectedStatus$ = this.selectedStatusSubject.asObservable();
  protected readonly selectedAssigneeId$ = this.selectedAssigneeIdSubject.asObservable();
  protected readonly currentPage$ = this.currentPageSubject.asObservable();
  protected readonly pageSize$ = this.pageSizeSubject.asObservable();
  protected readonly selectedTask$ = this.selectedTaskSubject.asObservable();

  protected readonly projectId$ = this.route.paramMap.pipe(
    map((params) => Number(params.get('id'))),
    shareReplay(1)
  );

  protected readonly selectedProject$ = combineLatest([
    this.projectId$,
    this.projectsApiService.getProjects()
  ]).pipe(
    map(([projectId, projects]: [number, ProjectSummaryDto[]]) =>
      projects.find(project => project.projectId === projectId) ?? null
    ),
    shareReplay(1)
  );

  protected readonly developers$ = this.developersApiService.getDevelopers().pipe(
    shareReplay(1)
  );

  protected readonly tasksResponse$ = combineLatest([
    this.projectId$,
    this.selectedStatus$,
    this.selectedAssigneeId$,
    this.currentPage$,
    this.pageSize$
  ]).pipe(
    switchMap(([projectId, status, assigneeId, page, pageSize]) =>
      this.projectsApiService.getProjectTasks(projectId, {
        page,
        pageSize,
        status,
        assigneeId
      })
    ),
    shareReplay(1)
  );

  protected readonly tasks$ = this.tasksResponse$.pipe(
    map((response) => response.items)
  );

  protected readonly totalCount$ = this.tasksResponse$.pipe(
    map((response) => response.totalCount)
  );

  protected readonly totalPages$ = this.tasksResponse$.pipe(
    map((response) => Math.ceil(response.totalCount / response.pageSize))
  );

  protected onStatusChange(value: string): void {
    const parsedValue = value === '' ? null : Number(value);

    this.selectedStatusSubject.next(parsedValue as TaskItemStatus | null);
    this.currentPageSubject.next(1);
  }

  protected onAssigneeChange(value: string): void {
    const parsedValue = value === '' ? null : Number(value);

    this.selectedAssigneeIdSubject.next(parsedValue);
    this.currentPageSubject.next(1);
  }

  protected goToPreviousPage(currentPage: number): void {
    if (currentPage > 1) {
      this.currentPageSubject.next(currentPage - 1);
    }
  }

  protected goToNextPage(currentPage: number, totalPages: number): void {
    if (currentPage < totalPages) {
      this.currentPageSubject.next(currentPage + 1);
    }
  }

  protected selectTask(task: ProjectTaskDto): void {
    this.selectedTaskSubject.next(task);
  }

  protected clearSelectedTask(): void {
    this.selectedTaskSubject.next(null);
  }

  protected trackByDeveloperId(_: number, developer: DeveloperDto): number {
    return developer.developerId;
  }

  protected trackByTaskId(_: number, task: ProjectTaskDto): number {
    return task.taskId;
  }
}