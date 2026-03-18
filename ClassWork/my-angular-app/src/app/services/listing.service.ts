import { Injectable, signal } from '@angular/core';

export interface Listing {
    id: number;
    title: string;
    location: string;
    image: string;
    price: number;
    rating: number;
    category: string;
    date: string;
}

@Injectable({
    providedIn: 'root'
})
export class ListingService {
    private listings = signal<Listing[]>([
        {
            id: 1,
            title: 'Oceanfront Villa',
            location: 'Malibu, California',
            image: 'https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=800&q=80',
            price: 850,
            rating: 4.9,
            category: 'Beach',
            date: 'May 1 - 6'
        },
        {
            id: 2,
            title: 'Modern Mountain Cabin',
            location: 'Aspen, Colorado',
            image: 'https://images.unsplash.com/photo-1542718610-a1d656d1884c?auto=format&fit=crop&w=800&q=80',
            price: 450,
            rating: 4.8,
            category: 'Cabins',
            date: 'Jun 12 - 17'
        },
        {
            id: 3,
            title: 'Desert Oasis',
            location: 'Joshua Tree, California',
            image: 'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?auto=format&fit=crop&w=800&q=80',
            price: 320,
            rating: 4.95,
            category: 'Desert',
            date: 'Aug 5 - 10'
        },
        {
            id: 4,
            title: 'City Penthouse',
            location: 'New York City, NY',
            image: 'https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?auto=format&fit=crop&w=800&q=80',
            price: 600,
            rating: 4.7,
            category: 'Iconic cities',
            date: 'Sep 20 - 25'
        },
        {
            id: 5,
            title: 'Rustic Forest Retreat',
            location: 'Portland, Oregon',
            image: 'https://images.unsplash.com/photo-1449156001433-41bb7f13904a?auto=format&fit=crop&w=800&q=80',
            price: 280,
            rating: 4.85,
            category: 'Forests',
            date: 'Oct 10 - 15'
        },
        {
            id: 6,
            title: 'Tropical Beach House',
            location: 'Maui, Hawaii',
            image: 'https://images.unsplash.com/photo-1499793983690-e29da59ef1c2?auto=format&fit=crop&w=800&q=80',
            price: 550,
            rating: 4.92,
            category: 'Beachfront',
            date: 'Dec 1 - 8'
        }
    ]);

    getListings() {
        return this.listings;
    }
}
