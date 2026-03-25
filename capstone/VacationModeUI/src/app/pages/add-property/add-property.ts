import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
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

  
  selectedFiles: File[] = [];
  imagePreviews: string[] = [];

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
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
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

  onImagesSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;

    const newFiles = Array.from(input.files);
    const combined = [...this.selectedFiles, ...newFiles];

    if (combined.length > 5) {
      this.errorMessage = 'You can select a maximum of 5 images.';
      return;
    }

    this.errorMessage = '';
    this.selectedFiles = combined;
    this.imagePreviews = [];

    this.selectedFiles.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imagePreviews.push(e.target?.result as string);
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(file);
    });
  }

  removeImage(index: number) {
    this.selectedFiles.splice(index, 1);
    this.imagePreviews.splice(index, 1);
  }

  onSubmit() {
    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;

    if (this.isEditMode && this.propertyId) {
      this.propertyService.updateProperty(this.propertyId, this.property).subscribe({
        next: () => {
          
          if (this.selectedFiles.length > 0) {
            this.propertyService.uploadImages(this.propertyId!, this.selectedFiles).subscribe({
              next: () => {
                this.isLoading = false;
                this.successMessage = 'Property updated with new images!';
                setTimeout(() => this.router.navigate(['/properties']), 1200);
              },
              error: () => {
                this.isLoading = false;
                this.successMessage = 'Property updated, but image upload failed.';
                setTimeout(() => this.router.navigate(['/properties']), 1500);
              }
            });
          } else {
            this.isLoading = false;
            this.successMessage = 'Property updated successfully!';
            setTimeout(() => this.router.navigate(['/properties']), 1200);
          }
        },
        error: () => {
          this.isLoading = false;
          this.errorMessage = 'Failed to update property. Please try again.';
        }
      });
    } else {
      this.propertyService.createProperty(this.property).subscribe({
        next: (created: any) => {
          const newId = created.propertyId ?? created.id;
          if (this.selectedFiles.length > 0 && newId) {
            this.propertyService.uploadImages(newId, this.selectedFiles).subscribe({
              next: () => {
                this.isLoading = false;
                this.successMessage = 'Property added with images!';
                setTimeout(() => this.router.navigate(['/properties']), 1200);
              },
              error: () => {
                this.isLoading = false;
                this.successMessage = 'Property added, but image upload failed. You can re-upload images by editing.';
                setTimeout(() => this.router.navigate(['/properties']), 2000);
              }
            });
          } else {
            this.isLoading = false;
            this.successMessage = 'Property added successfully!';
            setTimeout(() => this.router.navigate(['/properties']), 1200);
          }
        },
        error: () => {
          this.isLoading = false;
          this.errorMessage = 'Failed to add property. Please try again.';
        }
      });
    }
  }
}
