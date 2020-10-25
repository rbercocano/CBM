import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopFiveSalesComponent } from './top-five-sales.component';

describe('TopFiveSalesComponent', () => {
  let component: TopFiveSalesComponent;
  let fixture: ComponentFixture<TopFiveSalesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TopFiveSalesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopFiveSalesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
