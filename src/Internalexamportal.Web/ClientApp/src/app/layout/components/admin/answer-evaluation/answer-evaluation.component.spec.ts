import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AnswerEvaluationComponent } from './answer-evaluation.component';

describe('AnswerEvaluationComponent', () => {
  let component: AnswerEvaluationComponent;
  let fixture: ComponentFixture<AnswerEvaluationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AnswerEvaluationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AnswerEvaluationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
