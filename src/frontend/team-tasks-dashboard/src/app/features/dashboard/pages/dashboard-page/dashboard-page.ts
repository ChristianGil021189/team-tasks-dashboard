import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BehaviorSubject, combineLatest, map, shareReplay } from 'rxjs';

import { DashboardApiService } from '../../../../data-access/dashboard/dashboard-api.service';
import { DeveloperWorkloadDto } from '../../../../models/dashboard/dashboard.models';

type DeveloperWorkloadSortField = 'developerName' | 'openTasksCount';
type SortDirection = 'asc' | 'desc';

@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard-page.html',
  styleUrl: './dashboard-page.scss'
})
export class DashboardPage {
  private readonly dashboardApiService = inject(DashboardApiService);
  private readonly router = inject(Router);

  private readonly developerWorkloadSource$ = this.dashboardApiService
    .getDeveloperWorkload()
    .pipe(shareReplay(1));

  private readonly developerWorkloadSortFieldSubject =
    new BehaviorSubject<DeveloperWorkloadSortField>('developerName');

  private readonly developerWorkloadSortDirectionSubject =
    new BehaviorSubject<SortDirection>('asc');

  readonly developerWorkload$ = combineLatest([
    this.developerWorkloadSource$,
    this.developerWorkloadSortFieldSubject,
    this.developerWorkloadSortDirectionSubject
  ]).pipe(
    map(([items, field, direction]) =>
      this.sortDeveloperWorkload(items, field, direction)
    )
  );

  readonly projectHealth$ = this.dashboardApiService
    .getProjectHealth()
    .pipe(shareReplay(1));

  readonly developerDelayRisk$ = this.dashboardApiService
    .getDeveloperDelayRisk()
    .pipe(shareReplay(1));

  get developerWorkloadSortField(): DeveloperWorkloadSortField {
    return this.developerWorkloadSortFieldSubject.value;
  }

  get developerWorkloadSortDirection(): SortDirection {
    return this.developerWorkloadSortDirectionSubject.value;
  }

  setDeveloperWorkloadSort(field: DeveloperWorkloadSortField): void {
    if (this.developerWorkloadSortField === field) {
      this.developerWorkloadSortDirectionSubject.next(
        this.developerWorkloadSortDirection === 'asc' ? 'desc' : 'asc'
      );
      return;
    }

    this.developerWorkloadSortFieldSubject.next(field);
    this.developerWorkloadSortDirectionSubject.next('asc');
  }

  isDeveloperWorkloadSort(
    field: DeveloperWorkloadSortField,
    direction: SortDirection
  ): boolean {
    return (
      this.developerWorkloadSortField === field &&
      this.developerWorkloadSortDirection === direction
    );
  }

  goToProjectTasks(projectId: number): void {
    this.router.navigate(['/projects', projectId]);
  }

  private sortDeveloperWorkload(
    items: DeveloperWorkloadDto[],
    field: DeveloperWorkloadSortField,
    direction: SortDirection
  ): DeveloperWorkloadDto[] {
    const sortedItems = [...items].sort((left, right) => {
      if (field === 'developerName') {
        return left.developerName.localeCompare(right.developerName);
      }

      return left.openTasksCount - right.openTasksCount;
    });

    return direction === 'asc' ? sortedItems : sortedItems.reverse();
  }
}