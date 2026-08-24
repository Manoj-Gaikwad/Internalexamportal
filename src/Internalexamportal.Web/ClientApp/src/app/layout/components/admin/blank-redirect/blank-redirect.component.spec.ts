import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BlankRedirectComponent } from './blank-redirect.component';

describe('BlankRedirectComponent', () => {
  let component: BlankRedirectComponent;
  let fixture: ComponentFixture<BlankRedirectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BlankRedirectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BlankRedirectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
