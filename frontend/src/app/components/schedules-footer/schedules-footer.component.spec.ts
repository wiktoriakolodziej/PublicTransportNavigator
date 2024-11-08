import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulesFooterComponent } from './schedules-footer.component';

describe('SchedulesFooterComponent', () => {
  let component: SchedulesFooterComponent;
  let fixture: ComponentFixture<SchedulesFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SchedulesFooterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SchedulesFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
