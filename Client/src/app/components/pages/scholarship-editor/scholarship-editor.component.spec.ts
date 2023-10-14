import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScholarshipEditorComponent } from './scholarship-editor.component';

describe('ScholarshipEditorComponent', () => {
  let component: ScholarshipEditorComponent;
  let fixture: ComponentFixture<ScholarshipEditorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScholarshipEditorComponent]
    });
    fixture = TestBed.createComponent(ScholarshipEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
