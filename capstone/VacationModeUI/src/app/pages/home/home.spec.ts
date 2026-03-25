import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PLATFORM_ID } from '@angular/core';
import { HomeComponent } from './home';
import { PropertyService } from '../../services/property';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { vi } from 'vitest';
import { provideRouter } from '@angular/router';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let propertyService: PropertyService;
  let router: Router;

  beforeEach(async () => {
    const propertyServiceMock = {
      getAllProperties: vi.fn().mockReturnValue(of([]))
    };

    await TestBed.configureTestingModule({
      imports: [HomeComponent],
      providers: [
        { provide: PropertyService, useValue: propertyServiceMock },
        { provide: PLATFORM_ID, useValue: 'browser' },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    propertyService = TestBed.inject(PropertyService);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should process properties and group by city on init', () => {
    const mockProperties = [
      { city: 'Goa', rating: 5 },
      { city: 'Goa', rating: 4 },
      { city: 'Mumbai', rating: 4.5 },
      { city: 'Delhi', rating: 3 }
    ];
    vi.mocked(propertyService.getAllProperties).mockReturnValue(of(mockProperties as any));

    component.ngOnInit();

    expect(component.popularDestinations.length).toBeGreaterThan(0);
    const goa = component.popularDestinations.find(d => d.name === 'Goa');
    expect(goa?.maxRating).toBe(5);
    expect(goa?.icon).toBe('🌊');
  });

  it('should navigate to properties with city query param when a destination is clicked', () => {
    const navigateSpy = vi.spyOn(router, 'navigate');
    component.navigateToCity('Goa');
    expect(navigateSpy).toHaveBeenCalledWith(['/properties'], { queryParams: { city: 'Goa' } });
  });
});
