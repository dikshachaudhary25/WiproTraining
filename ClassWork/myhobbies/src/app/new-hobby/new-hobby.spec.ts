import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewHobby } from './new-hobby';

describe('NewHobby', () => {
  let component: NewHobby;
  let fixture: ComponentFixture<NewHobby>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NewHobby],
    }).compileComponents();

    fixture = TestBed.createComponent(NewHobby);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
