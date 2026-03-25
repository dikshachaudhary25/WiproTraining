import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyHobbies } from './my-hobbies';

describe('MyHobbies', () => {
  let component: MyHobbies;
  let fixture: ComponentFixture<MyHobbies>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MyHobbies],
    }).compileComponents();

    fixture = TestBed.createComponent(MyHobbies);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
