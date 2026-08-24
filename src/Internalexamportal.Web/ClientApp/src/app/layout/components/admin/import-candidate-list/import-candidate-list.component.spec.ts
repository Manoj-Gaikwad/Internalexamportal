import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportCandidateListComponent } from './import-candidate-list.component';

describe('ImportCandidateListComponent', () => {
  let component: ImportCandidateListComponent;
  let fixture: ComponentFixture<ImportCandidateListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportCandidateListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportCandidateListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
