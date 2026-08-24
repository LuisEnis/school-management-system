import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './pagination.html',
  styleUrl: './pagination.css'
})
export class PaginationComponent {

  @Input() pageNumber = 1;
  @Input() pageSize = 15;
  @Input() totalPages = 0;
  @Input() totalCount = 0;

  @Output() pageChange = new EventEmitter<number>();
  @Output() pageSizeChange = new EventEmitter<number>();

  pageSizeInput = 15;

  Math = Math;

  ngOnChanges(): void {
    this.pageSizeInput = this.pageSize;
  }

  get pageNumbers(): number[] {

  const maxVisiblePages = 5;

  if (this.totalPages <= maxVisiblePages) {
    return Array.from(
      { length: this.totalPages },
      (_, index) => index + 1
    );
  }

  let start = Math.max(
    1,
    this.pageNumber - 2
  );

  let end = Math.min(
    this.totalPages,
    start + maxVisiblePages - 1
  );

  if (end === this.totalPages) {
    start = Math.max(
      1,
      end - maxVisiblePages + 1
    );
  }

  return Array.from(
    { length: end - start + 1 },
    (_, index) => start + index
  );
}

  goToPage(page: number): void {

    if (
      page < 1 ||
      page > this.totalPages ||
      page === this.pageNumber
    ) {
      return;
    }

    this.pageChange.emit(page);
  }

  changePageSize(): void {

    const size = Number(this.pageSizeInput);

    if (!Number.isInteger(size) || size < 1) {
      this.pageSizeInput = this.pageSize;
      return;
    }

    this.pageSizeChange.emit(size);
  }

  onPageSizeSelect(event: Event): void {

  const select = event.target as HTMLSelectElement;

  const size = Number(select.value);

  this.pageSizeInput = size;

  this.pageSizeChange.emit(size);
}


onCustomPageSize(event: Event): void {

  const input = event.target as HTMLInputElement;

  const size = Number(input.value);

  if (!Number.isInteger(size) || size < 1) {

    input.value = this.pageSize.toString();
    this.pageSizeInput = this.pageSize;

    return;
  }

  this.pageSizeInput = size;

  this.pageSizeChange.emit(size);
}

}