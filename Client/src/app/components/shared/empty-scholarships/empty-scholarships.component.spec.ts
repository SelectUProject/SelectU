import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmptyScholarshipsComponent } from './empty-scholarships.component';

describe('EmptyScholarshipsComponent', () => {
  let component: EmptyScholarshipsComponent;
  let fixture: ComponentFixture<EmptyScholarshipsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EmptyScholarshipsComponent]
    });
    fixture = TestBed.createComponent(EmptyScholarshipsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
