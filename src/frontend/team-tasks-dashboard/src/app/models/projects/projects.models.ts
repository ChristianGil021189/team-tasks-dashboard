import { ProjectStatus, TaskItemStatus, TaskPriority } from '../shared/app.enums';

export interface ProjectSummaryDto {
  projectId: number;
  name: string;
  clientName: string;
  status: ProjectStatus;
  startDate: string;
  endDate: string | null;
  totalTasks: number;
  openTasks: number;
  completedTasks: number;
}

export interface ProjectTaskDto {
  taskId: number;
  projectId: number;
  title: string;
  description: string | null;
  assigneeId: number;
  assigneeFullName: string;
  status: TaskItemStatus;
  priority: TaskPriority;
  estimatedComplexity: number;
  createdAt: string;
  dueDate: string;
  completionDate: string | null;
}

export interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export interface ProjectTasksQuery {
  page: number;
  pageSize: number;
  status?: TaskItemStatus | null;
  assigneeId?: number | null;
}