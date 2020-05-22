import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderItemStatusLabelComponent } from './order-item-status-label.component';

describe('OrderItemStatusLabelComponent', () => {
  let component: OrderItemStatusLabelComponent;
  let fixture: ComponentFixture<OrderItemStatusLabelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderItemStatusLabelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderItemStatusLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
