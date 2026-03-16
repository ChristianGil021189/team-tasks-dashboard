import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize, forkJoin } from 'rxjs';

import { DevelopersApiService } from '../../../../data-access/developers/developers-api.service';
import { ProjectsApiService } from '../../../../data-access/projects/projects-api.service';
import { TasksApiService } from '../../../../data-access/tasks/tasks-api.service';
import { DeveloperDto } from '../../../../models/developers/developer.models';
import { ProjectSummaryDto } from '../../../../models/projects/projects.models';
import { TaskItemStatus, TaskPriority } from '../../../../models/shared/app.enums';
import { CreateTaskRequest } from '../../../../models/tasks/task.models';

@Component({
  selector: 'app-task-create-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './task-create-page.html',
  styleUrl: './task-create-page.scss'
})
export class TaskCreatePage implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly projectsApiService = inject(ProjectsApiService);
  private readonly developersApiService = inject(DevelopersApiService);
  private readonly tasksApiService = inject(TasksApiService);

  readonly statusOptions = [
    { value: TaskItemStatus.ToDo, label: 'To Do' },
    { value: TaskItemStatus.InProgress, label: 'In Progress' },
    { value: TaskItemStatus.Blocked, label: 'Blocked' },
    { value: TaskItemStatus.Completed, label: 'Completed' }
  ];

  readonly priorityOptions = [
    { value: TaskPriority.Low, label: 'Low' },
    { value: TaskPriority.Medium, label: 'Medium' },
    { value: TaskPriority.High, label: 'High' }
  ];

  readonly taskForm = this.formBuilder.group({
  projectId: this.formBuilder.control<number | null>(null, [
    Validators.required,
    Validators.min(1)
  ]),
  title: this.formBuilder.nonNullable.control('', [
    Validators.required,
    Validators.maxLength(150)
  ]),
  description: this.formBuilder.control<string | null>(null),
  assigneeId: this.formBuilder.control<number | null>(null, [
    Validators.required,
    Validators.min(1)
  ]),
  status: this.formBuilder.nonNullable.control(TaskItemStatus.ToDo, [
    Validators.required
  ]),
  priority: this.formBuilder.nonNullable.control(TaskPriority.Medium, [
    Validators.required
  ]),
  estimatedComplexity: this.formBuilder.nonNullable.control(1, [
    Validators.required,
    Validators.min(1),
    Validators.max(5)
  ]),
  dueDate: this.formBuilder.nonNullable.control('', [
    Validators.required
  ])
});

  projects: ProjectSummaryDto[] = [];
  developers: DeveloperDto[] = [];

  isLoadingData = false;
  isSubmitting = false;
  loadError = '';
  submitError = '';
  submitSuccess = '';

  ngOnInit(): void {
    this.loadFormData();
  }

  loadFormData(): void {
    this.isLoadingData = true;
    this.loadError = '';

    console.log('Loading form data...');

    forkJoin({
      projects: this.projectsApiService.getProjects(),
      developers: this.developersApiService.getDevelopers()
    })
      .pipe(
        finalize(() => {
          this.isLoadingData = false;
          console.log('Loading finished');
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: ({ projects, developers }) => {
          console.log('Projects:', projects);
          console.log('Developers:', developers);

          this.projects = projects;
          this.developers = developers;
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.error('Load error:', error);
          this.loadError = 'Could not load projects and developers.';
          this.cdr.detectChanges();
        }
      });
  }

  submit(): void {
  this.submitError = '';
  this.submitSuccess = '';

  if (this.taskForm.invalid) {
    this.taskForm.markAllAsTouched();
    return;
  }

  const formValue = this.taskForm.getRawValue();

  if (
    formValue.projectId === null ||
    formValue.assigneeId === null ||
    !formValue.title ||
    !formValue.dueDate
  ) {
    this.submitError = 'Please complete the required fields.';
    return;
  }

  const request: CreateTaskRequest = {
    projectId: formValue.projectId,
    title: formValue.title.trim(),
    description: formValue.description?.trim() || null,
    assigneeId: formValue.assigneeId,
    status: formValue.status,
    priority: formValue.priority,
    estimatedComplexity: formValue.estimatedComplexity,
    dueDate: formValue.dueDate
  };

  this.isSubmitting = true;

  this.tasksApiService
    .createTask(request)
    .pipe(
      finalize(() => {
        this.isSubmitting = false;
        this.cdr.detectChanges();
      })
    )
    .subscribe({
      next: (response) => {
        this.submitSuccess = `Task created successfully. ID: ${response.taskId}`;
        this.resetForm();
        this.cdr.detectChanges();
      },
      error: () => {
        this.submitError = 'Could not create the task.';
        this.cdr.detectChanges();
      }
    });
}

  hasError(
    controlName: keyof typeof this.taskForm.controls,
    errorName: string
  ): boolean {
    const control = this.taskForm.controls[controlName];
    return !!control && control.touched && control.hasError(errorName);
  }

  private resetForm(): void {
    this.taskForm.reset({
      projectId: null,
      title: '',
      description: null,
      assigneeId: null,
      status: TaskItemStatus.ToDo,
      priority: TaskPriority.Medium,
      estimatedComplexity: 1,
      dueDate: ''
    });
  }
}