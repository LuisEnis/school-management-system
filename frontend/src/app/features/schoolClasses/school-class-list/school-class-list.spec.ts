import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchoolClassList } from './school-class-list';

describe('SchoolClassList', () => {
  let component: SchoolClassList;
  let fixture: ComponentFixture<SchoolClassList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SchoolClassList],
    }).compileComponents();

    fixture = TestBed.createComponent(SchoolClassList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
