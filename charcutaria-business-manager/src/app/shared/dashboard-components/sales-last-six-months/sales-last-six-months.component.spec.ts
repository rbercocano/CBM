import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesLastSixMonthsComponent } from './sales-last-six-months.component';

describe('SalesLastSixMonthsComponent', () => {
  let component: SalesLastSixMonthsComponent;
  let fixture: ComponentFixture<SalesLastSixMonthsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalesLastSixMonthsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesLastSixMonthsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
