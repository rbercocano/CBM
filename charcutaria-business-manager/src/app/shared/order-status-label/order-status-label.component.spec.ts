import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderStatusLabelComponent } from './order-status-label.component';

describe('OrderStatusLabelComponent', () => {
  let component: OrderStatusLabelComponent;
  let fixture: ComponentFixture<OrderStatusLabelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderStatusLabelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderStatusLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
