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
  imageUrl?: string;
  features?: string[];
}