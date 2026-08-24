import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishModelComponent } from './finish-model.component';

describe('FinishModelComponent', () => {
  let component: FinishModelComponent;
  let fixture: ComponentFixture<FinishModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FinishModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FinishModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
