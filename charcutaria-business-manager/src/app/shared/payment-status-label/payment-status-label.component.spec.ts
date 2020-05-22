import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentStatusLabelComponent } from './payment-status-label.component';

describe('PaymentStatusLabelComponent', () => {
  let component: PaymentStatusLabelComponent;
  let fixture: ComponentFixture<PaymentStatusLabelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentStatusLabelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentStatusLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
