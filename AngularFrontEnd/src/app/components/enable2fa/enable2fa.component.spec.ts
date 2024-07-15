import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Enable2faComponent } from './enable2fa.component';

describe('Enable2faComponent', () => {
  let component: Enable2faComponent;
  let fixture: ComponentFixture<Enable2faComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Enable2faComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Enable2faComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
