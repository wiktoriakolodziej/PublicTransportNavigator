import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusStopPageComponent } from './bus-stop-page.component';

describe('BusStopPageComponent', () => {
  let component: BusStopPageComponent;
  let fixture: ComponentFixture<BusStopPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BusStopPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BusStopPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
