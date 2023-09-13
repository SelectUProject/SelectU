import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShortViewScholarshipsComponentComponent } from './short-view-scholarships-component.component';

describe('ShortViewScholarshipsComponentComponent', () => {
  let component: ShortViewScholarshipsComponentComponent;
  let fixture: ComponentFixture<ShortViewScholarshipsComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShortViewScholarshipsComponentComponent]
    });
    fixture = TestBed.createComponent(ShortViewScholarshipsComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
