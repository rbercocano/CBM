import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SummarizedProductionComponent } from './summarized-production.component';

describe('SummarizedProductionComponent', () => {
  let component: SummarizedProductionComponent;
  let fixture: ComponentFixture<SummarizedProductionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SummarizedProductionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SummarizedProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
