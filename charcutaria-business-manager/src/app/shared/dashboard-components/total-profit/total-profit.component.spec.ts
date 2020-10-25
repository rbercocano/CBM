import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TotalProfitComponent } from './total-profit.component';

describe('TotalProfitComponent', () => {
  let component: TotalProfitComponent;
  let fixture: ComponentFixture<TotalProfitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TotalProfitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TotalProfitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
