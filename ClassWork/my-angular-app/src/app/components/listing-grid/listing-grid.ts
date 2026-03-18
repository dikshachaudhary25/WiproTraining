import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListingService } from '../../services/listing.service';
import { ListingCardComponent } from '../listing-card/listing-card';

@Component({
    selector: 'app-listing-grid',
    standalone: true,
    imports: [CommonModule, ListingCardComponent],
    templateUrl: './listing-grid.html',
    styleUrl: './listing-grid.css'
})
export class ListingGridComponent {
    private listingService = inject(ListingService);
    listings = this.listingService.getListings();
}
