import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Listing } from '../../services/listing.service';

@Component({
    selector: 'app-listing-card',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './listing-card.html',
    styleUrl: './listing-card.css'
})
export class ListingCardComponent {
    listing = input.required<Listing>();
}
