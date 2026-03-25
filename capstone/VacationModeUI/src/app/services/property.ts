import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Property } from '../models/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {

  private apiUrl = '/api/property';

  constructor(private http: HttpClient) { }

  getAllProperties(): Observable<Property[]> {
    return this.http.get<Property[]>(this.apiUrl);
  }

  searchProperties(params: any): Observable<Property[]> {
    let queryParams = new URLSearchParams();
    if (params.city) queryParams.append('city', params.city);
    if (params.type) queryParams.append('type', params.type);
    if (params.minPrice) queryParams.append('minPrice', params.minPrice.toString());
    if (params.maxPrice) queryParams.append('maxPrice', params.maxPrice.toString());
    if (params.checkIn) queryParams.append('checkIn', params.checkIn);
    if (params.checkOut) queryParams.append('checkOut', params.checkOut);
    if (params.features) queryParams.append('features', params.features);

    return this.http.get<Property[]>(`${this.apiUrl}/search?${queryParams.toString()}`);
  }

  getPropertyById(id: number): Observable<Property> {
    return this.http.get<Property>(`${this.apiUrl}/${id}`);
  }

  createProperty(data: Property): Observable<Property> {
    return this.http.post<Property>(this.apiUrl, data);
  }

  updateProperty(id: number, data: Property): Observable<Property> {
    return this.http.put<Property>(`${this.apiUrl}/${id}`, data);
  }

  deleteProperty(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  uploadImages(propertyId: number, files: File[]): Observable<{ uploaded: number; urls: string[] }> {
    const formData = new FormData();
    files.forEach(f => formData.append('images', f, f.name));
    return this.http.post<{ uploaded: number; urls: string[] }>(
      `/api/upload/property/${propertyId}`,
      formData
    );
  }

  deleteImage(imageId: number): Observable<void> {
    return this.http.delete<void>(`/api/upload/property-image/${imageId}`);
  }
}
