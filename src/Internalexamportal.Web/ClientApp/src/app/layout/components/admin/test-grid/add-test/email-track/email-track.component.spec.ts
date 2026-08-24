import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailTrackComponent } from './email-track.component';

describe('EmailTrackComponent', () => {
  let component: EmailTrackComponent;
  let fixture: ComponentFixture<EmailTrackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailTrackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailTrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
