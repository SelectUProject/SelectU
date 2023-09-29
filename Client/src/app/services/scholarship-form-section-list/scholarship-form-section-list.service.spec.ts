import { TestBed } from '@angular/core/testing';

import { ScholarshipFormSectionListService } from './scholarship-form-section-list.service';

describe('ScholarshipFormSectionListService', () => {
  let service: ScholarshipFormSectionListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScholarshipFormSectionListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
