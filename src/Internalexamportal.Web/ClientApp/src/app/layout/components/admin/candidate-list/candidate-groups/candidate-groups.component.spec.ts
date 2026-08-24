import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateGroupsComponent } from './candidate-groups.component';

describe('CandidateGroupsComponent', () => {
  let component: CandidateGroupsComponent;
  let fixture: ComponentFixture<CandidateGroupsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateGroupsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
