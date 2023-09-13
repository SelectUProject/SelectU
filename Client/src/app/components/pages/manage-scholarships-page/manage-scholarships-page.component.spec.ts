import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageScholarshipsPageComponent } from './manage-scholarships-page.component';

describe('ManageScholarshipsPageComponent', () => {
  let component: ManageScholarshipsPageComponent;
  let fixture: ComponentFixture<ManageScholarshipsPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ManageScholarshipsPageComponent]
    });
    fixture = TestBed.createComponent(ManageScholarshipsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
