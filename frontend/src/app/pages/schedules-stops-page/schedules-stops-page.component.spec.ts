import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulesStopsPageComponent } from './schedules-stops-page.component';

describe('SchedulesStopsPageComponent', () => {
  let component: SchedulesStopsPageComponent;
  let fixture: ComponentFixture<SchedulesStopsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SchedulesStopsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SchedulesStopsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
