import { Pipe, PipeTransform } from '@angular/core';

import {
  ProjectStatus,
  TaskItemStatus,
  TaskPriority
} from '../../models/shared/app.enums';

type EnumLabelType = 'projectStatus' | 'taskStatus' | 'taskPriority';

@Pipe({
  name: 'enumLabel',
  standalone: true
})
export class EnumLabelPipe implements PipeTransform {
  private readonly enumMap = {
    projectStatus: ProjectStatus,
    taskStatus: TaskItemStatus,
    taskPriority: TaskPriority
  } as const;

  transform(value: number | null | undefined, type: EnumLabelType): string {
    if (value === null || value === undefined) {
      return '-';
    }

    const enumObject = this.enumMap[type];
    const enumKey = enumObject[value as keyof typeof enumObject];

    if (typeof enumKey !== 'string') {
      return '-';
    }

    return this.formatEnumKey(enumKey);
  }

  private formatEnumKey(value: string): string {
    return value.replaceAll(/([a-z])([A-Z])/g, '$1 $2');
  }
}