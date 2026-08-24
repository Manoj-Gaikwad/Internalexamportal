import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivationcodeComponent } from './activationcode.component';

describe('ActivationcodeComponent', () => {
  let component: ActivationcodeComponent;
  let fixture: ComponentFixture<ActivationcodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActivationcodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActivationcodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
