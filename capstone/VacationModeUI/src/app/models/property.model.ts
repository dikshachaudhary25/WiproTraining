export interface Property {
  propertyId?: number;
  ownerId?: number;
  ownerName?: string;
  title: string;
  description: string;
  location: string;
  city: string;
  state: string;
  country: string;
  propertyType: string;
  pricePerNight: number;
  maxGuests: number;
  createdAt?: Date;
  images?: string[];
  rating?: number;
  features?: string[];
  isInWishlist?: boolean;
}