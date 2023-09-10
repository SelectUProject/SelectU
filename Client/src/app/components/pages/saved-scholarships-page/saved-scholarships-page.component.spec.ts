import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SavedScholarshipsPageComponent } from './saved-scholarships-page.component';

describe('SavedScholarshipsPageComponent', () => {
  let component: SavedScholarshipsPageComponent;
  let fixture: ComponentFixture<SavedScholarshipsPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SavedScholarshipsPageComponent]
    });
    fixture = TestBed.createComponent(SavedScholarshipsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
