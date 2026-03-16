import { CommonModule } from '@angular/common';
import {
  Component,
  TemplateRef,
  computed,
  inject,
  viewChild
} from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BehaviorSubject, combineLatest, map, shareReplay } from 'rxjs';

import { DashboardApiService } from '../../../../data-access/dashboard/dashboard-api.service';
import {
  DeveloperWorkloadDto,
  ProjectHealthDto
} from '../../../../models/dashboard/dashboard.models';
import {
  DataTableCellTemplateContext,
  DataTableColumn,
  DataTableComponent,
  DataTableHeaderTemplateContext
} from '../../../../shared/components/data-table/data-table';

type DeveloperWorkloadSortField = 'developerName' | 'openTasksCount';
type SortDirection = 'asc' | 'desc';

@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  imports: [CommonModule, RouterLink, DataTableComponent],
  templateUrl: './dashboard-page.html',
  styleUrl: './dashboard-page.scss'
})
export class DashboardPage {
  private readonly dashboardApiService = inject(DashboardApiService);
  private readonly router = inject(Router);

  private readonly developerNameHeaderTemplate =
    viewChild<TemplateRef<DataTableHeaderTemplateContext<DeveloperWorkloadDto>>>(
      'developerNameHeaderTemplate'
    );

  private readonly openTasksHeaderTemplate =
    viewChild<TemplateRef<DataTableHeaderTemplateContext<DeveloperWorkloadDto>>>(
      'openTasksHeaderTemplate'
    );

  private readonly projectHealthActionsCellTemplate =
    viewChild<TemplateRef<DataTableCellTemplateContext<ProjectHealthDto>>>(
      'projectHealthActionsCellTemplate'
    );

  private readonly developerWorkloadSource$ = this.dashboardApiService
    .getDeveloperWorkload()
    .pipe(shareReplay(1));

  private readonly developerWorkloadSortFieldSubject =
    new BehaviorSubject<DeveloperWorkloadSortField>('developerName');

  private readonly developerWorkloadSortDirectionSubject =
    new BehaviorSubject<SortDirection>('asc');

  readonly developerWorkloadColumns = computed<
    readonly DataTableColumn<DeveloperWorkloadDto>[]
  >(() => [
    {
      key: 'developerName',
      header: 'Developer',
      headerTemplate: this.developerNameHeaderTemplate() ?? undefined
    },
    {
      key: 'openTasksCount',
      header: 'Open Tasks',
      headerTemplate: this.openTasksHeaderTemplate() ?? undefined
    },
    {
      key: 'averageEstimatedComplexity',
      header: 'Avg. Complexity',
      formatter: (value) => this.formatAverageEstimatedComplexity(value)
    }
  ]);

  readonly projectHealthColumns = computed<
    readonly DataTableColumn<ProjectHealthDto>[]
  >(() => [
    {
      key: 'projectName',
      header: 'Project'
    },
    {
      key: 'clientName',
      header: 'Client'
    },
    {
      key: 'totalTasks',
      header: 'Total'
    },
    {
      key: 'openTasks',
      header: 'Open'
    },
    {
      key: 'completedTasks',
      header: 'Completed'
    },
    {
      key: 'actions',
      header: 'Actions',
      cellTemplate: this.projectHealthActionsCellTemplate() ?? undefined
    }
  ]);

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

  getProjectHealthRowClass(row: object): string {
    const project = row as ProjectHealthDto;

    return project.openTasks > project.completedTasks
      ? 'table-row--warning'
      : '';
  }

  goToProjectTasks(projectId: number): void {
    this.router.navigate(['/projects', projectId]);
  }

  private formatAverageEstimatedComplexity(value: unknown): string {
    if (typeof value !== 'number' || Number.isNaN(value)) {
      return '—';
    }

    return new Intl.NumberFormat(undefined, {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2
    }).format(value);
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