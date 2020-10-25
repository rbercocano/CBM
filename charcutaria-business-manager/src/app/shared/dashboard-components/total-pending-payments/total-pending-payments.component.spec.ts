import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TotalPendingPaymentsComponent } from './total-pending-payments.component';

describe('TotalPendingPaymentsComponent', () => {
  let component: TotalPendingPaymentsComponent;
  let fixture: ComponentFixture<TotalPendingPaymentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TotalPendingPaymentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TotalPendingPaymentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
