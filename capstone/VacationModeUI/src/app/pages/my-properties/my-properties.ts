import { Component, OnInit, Inject, PLATFORM_ID, ChangeDetectorRef } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PropertyService } from '../../services/property';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-my-properties',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-properties.html',
  styleUrls: ['./my-properties.css']
})
export class MyPropertiesComponent implements OnInit {
  properties: Property[] = [];
  isLoading = true;
  errorMessage = '';
  propertyToDelete: Property | null = null;
  isDeleting = false;

  constructor(
    private propertyService: PropertyService,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    
    if (isPlatformBrowser(this.platformId)) {
      this.loadProperties();
    } else {
      this.isLoading = false;
    }
  }

  loadProperties() {
    this.isLoading = true;
    this.propertyService.getAllProperties().subscribe({
      next: (all: any[]) => {
        const userId = this.getLoggedInUserId();
        this.properties = all.filter(p => p.ownerId === userId);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load properties.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  getLoggedInUserId(): number {
    try {
      const token = localStorage.getItem('token');
      if (!token) return -1;
      const payload = JSON.parse(atob(token.split('.')[1]));
      return parseInt(
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ??
        payload['sub'] ??
        '-1'
      );
    } catch {
      return -1;
    }
  }

  getMainImage(property: any): string {
    if (property.images && property.images.length > 0) {
      const url = property.images[0];
      
      
      return url.startsWith('http') ? url : url;
    }
    return 'https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=600&auto=format&fit=crop&q=80';
  }

  confirmDelete(property: Property) {
    this.propertyToDelete = property;
  }

  cancelDelete() {
    this.propertyToDelete = null;
  }

  deleteProperty() {
    if (!this.propertyToDelete?.propertyId) return;
    this.isDeleting = true;
    this.propertyService.deleteProperty(this.propertyToDelete.propertyId).subscribe({
      next: () => {
        this.properties = this.properties.filter(p => p.propertyId !== this.propertyToDelete!.propertyId);
        this.propertyToDelete = null;
        this.isDeleting = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isDeleting = false;
        this.errorMessage = 'Failed to delete property. Please try again.';
        this.propertyToDelete = null;
        this.cdr.detectChanges();
      }
    });
  }
}
