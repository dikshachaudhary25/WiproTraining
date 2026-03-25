import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Hobby } from '../services/hobby';

@Component({
  selector: 'app-fav-hobby',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fav-hobby.html'
})
export class FavHobbyComponent {
  @Input() hobbies: Hobby[] = [];
}