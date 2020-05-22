import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerSidePagerComponent } from './server-side-pager.component';

describe('ServerSidePagerComponent', () => {
  let component: ServerSidePagerComponent;
  let fixture: ComponentFixture<ServerSidePagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerSidePagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerSidePagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
