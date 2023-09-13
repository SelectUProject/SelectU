import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FindScholarshipsComponent } from './find-scholarships.component';

describe('FindScholarshipsComponent', () => {
  let component: FindScholarshipsComponent;
  let fixture: ComponentFixture<FindScholarshipsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FindScholarshipsComponent]
    });
    fixture = TestBed.createComponent(FindScholarshipsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
