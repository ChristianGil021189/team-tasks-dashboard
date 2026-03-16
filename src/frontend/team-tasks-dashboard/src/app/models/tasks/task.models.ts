import { TaskItemStatus, TaskPriority } from '../shared/app.enums';

export interface CreateTaskRequest {
  projectId: number;
  title: string;
  description: string | null;
  assigneeId: number;
  status: TaskItemStatus;
  priority: TaskPriority;
  estimatedComplexity: number;
  dueDate: string;
}

export interface CreateTaskResponse {
  taskId: number;
}