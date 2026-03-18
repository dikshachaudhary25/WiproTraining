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
}
