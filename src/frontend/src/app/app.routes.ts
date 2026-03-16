import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/pages/dashboard-page/dashboard-page').then(
        (m) => m.DashboardPage
      )
  },
  {
    path: 'projects/:id',
    loadComponent: () =>
      import('./features/project-tasks/pages/project-tasks-page/project-tasks-page').then(
        (m) => m.ProjectTasksPage
      )
  },
  {
    path: 'tasks/new',
    loadComponent: () =>
      import('./features/task-create/pages/task-create-page/task-create-page').then(
        (m) => m.TaskCreatePage
      )
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];