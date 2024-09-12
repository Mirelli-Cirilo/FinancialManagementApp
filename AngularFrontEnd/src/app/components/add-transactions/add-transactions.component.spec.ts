import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTransactionsComponent } from './add-transactions.component';

describe('AddTransactionsComponent', () => {
  let component: AddTransactionsComponent;
  let fixture: ComponentFixture<AddTransactionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddTransactionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
