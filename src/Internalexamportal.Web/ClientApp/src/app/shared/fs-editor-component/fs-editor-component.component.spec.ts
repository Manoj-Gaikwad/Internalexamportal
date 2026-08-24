import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FSEditorComponent } from './fs-editor-component.component';

describe('FSEditorComponent', () => {
  let component: FSEditorComponent;
  let fixture: ComponentFixture<FSEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FSEditorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FSEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
