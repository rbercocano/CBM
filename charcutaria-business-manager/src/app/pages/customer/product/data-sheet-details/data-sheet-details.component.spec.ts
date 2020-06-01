import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataSheetDetailsComponent } from './data-sheet-details.component';

describe('DataSheetDetailsComponent', () => {
  let component: DataSheetDetailsComponent;
  let fixture: ComponentFixture<DataSheetDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataSheetDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataSheetDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
