import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { PropertyService } from '../../services/property';

interface PopularCity {
  name: string;
  icon: string;
  maxRating: number;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  popularDestinations: PopularCity[] = [];
  isLoading = true;

  private cityIcons: Record<string, string> = {
    'Goa': '🌊',
    'Manali': '🏔️',
    'Jaipur': '🏰',
    'Udaipur': '🏞️',
    'Kerala': '🌴',
    'Shimla': '❄️',
    'Delhi': '🏛️',
    'Mumbai': '🌆'
  };

  constructor(
    private propertyService: PropertyService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.propertyService.getAllProperties().subscribe({
        next: (all: any[]) => {
          const cityMap: Record<string, number> = {};
          
          
          all.forEach(p => {
            if (!p.city) return;
            const rating = p.rating || 0;
            if (!cityMap[p.city] || rating > cityMap[p.city]) {
              cityMap[p.city] = rating;
            }
          });

          
          let destinations = Object.entries(cityMap)
            .map(([name, maxRating]) => ({
              name,
              icon: this.cityIcons[name] || '📍',
              maxRating
            }))
            .sort((a, b) => b.maxRating - a.maxRating); 

          
          const highRated = destinations.filter(c => c.maxRating >= 4.0);
          this.popularDestinations = (highRated.length >= 3 ? highRated : destinations).slice(0, 6);

          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: () => { 
          this.isLoading = false; 
          this.cdr.detectChanges();
        }
      });
    } else {
      this.isLoading = false;
    }
  }

  navigateToCity(city: string) {
    this.router.navigate(['/properties'], { queryParams: { city } });
  }
}
