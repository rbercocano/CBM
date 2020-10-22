import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCorpClientComponent } from './edit-corp-client.component';

describe('EditCorpClientComponent', () => {
  let component: EditCorpClientComponent;
  let fixture: ComponentFixture<EditCorpClientComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCorpClientComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCorpClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
