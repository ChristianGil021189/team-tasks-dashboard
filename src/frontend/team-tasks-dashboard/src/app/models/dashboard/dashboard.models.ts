export interface DeveloperWorkloadDto {
  developerName: string;
  openTasksCount: number;
  averageEstimatedComplexity: number;
}

export interface ProjectHealthDto {
  projectId: number;
  projectName: string;
  clientName: string;
  totalTasks: number;
  openTasks: number;
  completedTasks: number;
  inProgressTasks: number;
  blockedTasks: number;
  overdueTasks: number;
  completionPercentage: number;
  riskPercentage: number;
}

export interface DeveloperDelayRiskDto {
  developerName: string;
  openTasksCount: number;
  avgDelayDays: number;
  nearestDueDate: string | null;
  latestDueDate: string | null;
  predictedCompletionDate: string | null;
  highRiskFlag: boolean;
}