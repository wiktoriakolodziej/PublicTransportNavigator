import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusLinesListComponent } from './bus-lines-list.component';

describe('BusLinesListComponent', () => {
  let component: BusLinesListComponent;
  let fixture: ComponentFixture<BusLinesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BusLinesListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BusLinesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
