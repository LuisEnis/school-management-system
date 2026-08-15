import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecretaryList } from './secretary-list';

describe('SecretaryList', () => {
  let component: SecretaryList;
  let fixture: ComponentFixture<SecretaryList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecretaryList],
    }).compileComponents();

    fixture = TestBed.createComponent(SecretaryList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
