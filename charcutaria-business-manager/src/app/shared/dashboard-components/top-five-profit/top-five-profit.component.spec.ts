import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopFiveProfitComponent } from './top-five-profit.component';

describe('TopFiveProfitComponent', () => {
  let component: TopFiveProfitComponent;
  let fixture: ComponentFixture<TopFiveProfitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TopFiveProfitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopFiveProfitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
