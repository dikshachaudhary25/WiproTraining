import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Hobby {
  id?: number;
  name: string;
  fav: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class HobbyService {

  private apiUrl = 'http://localhost:3000/hobbies';

  constructor(private http: HttpClient) {}

  getHobbies(): Observable<Hobby[]> {
    return this.http.get<Hobby[]>(this.apiUrl);
  }

  addHobby(hobby: Hobby): Observable<Hobby> {
    return this.http.post<Hobby>(this.apiUrl, hobby);
  }

  updateHobby(hobby: Hobby): Observable<Hobby> {
    return this.http.put<Hobby>(`${this.apiUrl}/${hobby.id}`, hobby);
  }

  deleteHobby(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}