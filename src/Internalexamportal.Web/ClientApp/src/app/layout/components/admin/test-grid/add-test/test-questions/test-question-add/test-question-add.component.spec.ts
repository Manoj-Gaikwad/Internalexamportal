import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestQuestionAddComponent } from './test-question-add.component';

describe('TestQuestionAddComponent', () => {
  let component: TestQuestionAddComponent;
  let fixture: ComponentFixture<TestQuestionAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestQuestionAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestQuestionAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
