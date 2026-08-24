import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditImportCandidateComponent } from './edit-import-candidate.component';

describe('EditImportCandidateComponent', () => {
  let component: EditImportCandidateComponent;
  let fixture: ComponentFixture<EditImportCandidateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditImportCandidateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditImportCandidateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
