import type { Property } from './property.model';

export interface Reservation {
  reservationId: number;
  userId: number;
  propertyId: number;
  checkInDate: string;
  checkOutDate: string;
  totalAmount: number;
  reservationStatus: string;
  createdAt: string;
  renterName?: string;
  property?: Property;
}
