import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavHobby } from './fav-hobby';

describe('FavHobby', () => {
  let component: FavHobby;
  let fixture: ComponentFixture<FavHobby>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FavHobby],
    }).compileComponents();

    fixture = TestBed.createComponent(FavHobby);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
