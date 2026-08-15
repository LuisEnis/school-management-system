import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecretaryForm } from './secretary-form';

describe('SecretaryForm', () => {
  let component: SecretaryForm;
  let fixture: ComponentFixture<SecretaryForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecretaryForm],
    }).compileComponents();

    fixture = TestBed.createComponent(SecretaryForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
