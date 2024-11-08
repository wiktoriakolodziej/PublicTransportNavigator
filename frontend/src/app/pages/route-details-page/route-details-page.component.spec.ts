import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RouteDetailsPageComponent } from './route-details-page.component';

describe('RouteDetailsPageComponent', () => {
  let component: RouteDetailsPageComponent;
  let fixture: ComponentFixture<RouteDetailsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouteDetailsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RouteDetailsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
