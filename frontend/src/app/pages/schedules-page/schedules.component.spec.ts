import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulesPageComponent } from './schedules.component';

describe('SchedulesComponent', () => {
  let component: SchedulesPageComponent;
  let fixture: ComponentFixture<SchedulesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SchedulesPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SchedulesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
