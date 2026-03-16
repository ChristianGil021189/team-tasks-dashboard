import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  TemplateRef,
  input
} from '@angular/core';

export type DataTableAlign = 'left' | 'center' | 'right';

export interface DataTableHeaderTemplateContext<T extends object = object> {
  $implicit: DataTableColumn<T>;
  column: DataTableColumn<T>;
}

export interface DataTableCellTemplateContext<T extends object = object> {
  $implicit: T;
  row: T;
  value: unknown;
  column: DataTableColumn<T>;
}

export interface DataTableColumn<T extends object = object> {
  key: keyof T | string;
  header: string;
  align?: DataTableAlign;
  valueGetter?: (row: T) => unknown;
  formatter?: (value: unknown, row: T) => string;
  cellClass?: string | ((value: unknown, row: T) => string);
  headerClass?: string;
  headerTemplate?: TemplateRef<DataTableHeaderTemplateContext<T>>;
  cellTemplate?: TemplateRef<DataTableCellTemplateContext<T>>;
}

type InternalDataTableColumn = DataTableColumn<any>;
type InternalRow = object;

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './data-table.html',
  styleUrl: './data-table.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DataTableComponent {
  readonly columns = input.required<readonly InternalDataTableColumn[]>();
  readonly rows = input.required<readonly InternalRow[]>();
  readonly emptyMessage = input('No records found.');
  readonly rowClass = input<string | ((row: InternalRow) => string)>('');

  trackByColumn(index: number, column: InternalDataTableColumn): string {
    return `${index}-${String(column.key)}`;
  }

  trackByRow(index: number): number {
    return index;
  }

  getHeaderContext(column: InternalDataTableColumn): DataTableHeaderTemplateContext<any> {
    return {
      $implicit: column,
      column
    };
  }

  getCellContext(
    row: InternalRow,
    column: InternalDataTableColumn
  ): DataTableCellTemplateContext<any> {
    const value = this.getCellValue(row, column);

    return {
      $implicit: row,
      row,
      value,
      column
    };
  }

  getRowClass(row: InternalRow): string {
    const currentRowClass = this.rowClass();

    if (typeof currentRowClass === 'function') {
      return currentRowClass(row);
    }

    return currentRowClass ?? '';
  }

  getCellText(row: InternalRow, column: InternalDataTableColumn): string {
    const value = this.getCellValue(row, column);

    if (column.formatter) {
      return column.formatter(value, row);
    }

    if (value === null || value === undefined || value === '') {
      return '—';
    }

    if (
      typeof value === 'string' ||
      typeof value === 'number' ||
      typeof value === 'boolean' ||
      typeof value === 'bigint'
    ) {
      return String(value);
    }

    return '—';
  }

  getCellClass(row: InternalRow, column: InternalDataTableColumn): string {
    const value = this.getCellValue(row, column);

    if (typeof column.cellClass === 'function') {
      return column.cellClass(value, row);
    }

    return column.cellClass ?? '';
  }

  private getCellValue(row: InternalRow, column: InternalDataTableColumn): unknown {
    if (column.valueGetter) {
      return column.valueGetter(row);
    }

    const rowRecord = row as Record<string, unknown>;
    return rowRecord[String(column.key)];
  }
}