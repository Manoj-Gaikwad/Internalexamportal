import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicAddEditComponent } from './topic-add-edit.component';

describe('TopicAddEditComponent', () => {
  let component: TopicAddEditComponent;
  let fixture: ComponentFixture<TopicAddEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TopicAddEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
