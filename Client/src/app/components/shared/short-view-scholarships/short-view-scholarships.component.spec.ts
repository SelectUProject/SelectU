import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShortViewScholarshipsComponent } from './short-view-scholarships.component';

describe('ShortViewScholarshipsComponent', () => {
  let component: ShortViewScholarshipsComponent;
  let fixture: ComponentFixture<ShortViewScholarshipsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShortViewScholarshipsComponent],
    });
    fixture = TestBed.createComponent(ShortViewScholarshipsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
