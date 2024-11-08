import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusLinePageComponent } from './bus-line-page.component';

describe('BusLinePageComponent', () => {
  let component: BusLinePageComponent;
  let fixture: ComponentFixture<BusLinePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BusLinePageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BusLinePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
