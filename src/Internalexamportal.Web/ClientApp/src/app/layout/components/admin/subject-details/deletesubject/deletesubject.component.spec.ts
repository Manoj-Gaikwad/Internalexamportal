import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeletesubjectComponent } from './deletesubject.component';

describe('DeletesubjectComponent', () => {
  let component: DeletesubjectComponent;
  let fixture: ComponentFixture<DeletesubjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeletesubjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeletesubjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
