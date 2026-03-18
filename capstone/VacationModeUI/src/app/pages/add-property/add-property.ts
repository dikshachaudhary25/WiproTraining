import { Component, OnInit, afterNextRender, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { PropertyService } from '../../services/property';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-add-property',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './add-property.html',
  styleUrls: ['./add-property.css']
})
export class AddPropertyComponent implements OnInit {

  isEditMode = false;
  propertyId: number | null = null;
  successMessage = '';
  errorMessage = '';
  isLoading = false;

  property: Property = {
    title: '',
    description: '',
    location: '',
    city: '',
    state: '',
    country: 'India',
    propertyType: 'Apartment',
    pricePerNight: 0,
    maxGuests: 1
  };

  constructor(
    private propertyService: PropertyService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {
    // Fetch property data only after the component renders in the browser
    afterNextRender(() => {
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        this.isEditMode = true;
        this.propertyId = +id;
        this.propertyService.getPropertyById(this.propertyId).subscribe({
          next: (data) => {
            this.property = {
              title: data.title,
              description: data.description,
              location: data.location,
              city: data.city,
              state: data.state,
              country: data.country,
              propertyType: data.propertyType,
              pricePerNight: data.pricePerNight,
              maxGuests: data.maxGuests
            };
            this.cdr.detectChanges();
          },
          error: () => {
            this.errorMessage = 'Failed to load property for editing.';
            this.cdr.detectChanges();
          }
        });
      }
    });
  }

  ngOnInit() {
    // intentionally empty — data loading is deferred to afterNextRender
  }

  onSubmit() {
    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;

    if (this.isEditMode && this.propertyId) {
      this.propertyService.updateProperty(this.propertyId, this.property).subscribe({
        next: () => {
          this.isLoading = false;
          this.successMessage = 'Property updated successfully!';
          setTimeout(() => this.router.navigate(['/properties']), 1200);
        },
        error: () => {
          this.isLoading = false;
          this.errorMessage = 'Failed to update property. Please try again.';
        }
      });
    } else {
      this.propertyService.createProperty(this.property).subscribe({
        next: () => {
          this.isLoading = false;
          this.successMessage = 'Property added successfully!';
          setTimeout(() => this.router.navigate(['/properties']), 1200);
        },
        error: () => {
          this.isLoading = false;
          this.errorMessage = 'Failed to add property. Please try again.';
        }
      });
    }
  }
}
