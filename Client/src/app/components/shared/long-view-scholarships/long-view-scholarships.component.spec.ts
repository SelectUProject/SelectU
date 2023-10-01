import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LongViewScholarshipsComponent } from './long-view-scholarships.component';

describe('LongViewScholarshipsComponent', () => {
  let component: LongViewScholarshipsComponent;
  let fixture: ComponentFixture<LongViewScholarshipsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LongViewScholarshipsComponent]
    });
    fixture = TestBed.createComponent(LongViewScholarshipsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
