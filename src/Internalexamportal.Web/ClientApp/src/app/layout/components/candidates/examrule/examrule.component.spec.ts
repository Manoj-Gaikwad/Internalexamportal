import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExamruleComponent } from './examrule.component';

describe('ExamruleComponent', () => {
  let component: ExamruleComponent;
  let fixture: ComponentFixture<ExamruleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExamruleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExamruleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
