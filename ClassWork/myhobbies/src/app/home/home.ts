import { Component, OnInit } from '@angular/core';
import { AuthService, Hobby } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { MyHobbiesComponent } from '../my-hobbies/my-hobbies';
import { NewHobbyComponent } from '../new-hobby/new-hobby';
import { FavHobbyComponent } from '../fav-hobby/fav-hobby';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MyHobbiesComponent, NewHobbyComponent, FavHobbyComponent],
  templateUrl: './home.html'
})
export class HomeComponent implements OnInit {
  hobbies: Hobby[] = [];

  constructor(public authService: AuthService) {}

  ngOnInit() {
    this.loadHobbies();
  }

  get selectedView(): string {
    return this.authService.selectedView;
  }

  loadHobbies() {
    this.hobbies = this.authService.getHobbies();
  }

  addHobby(hobby: Hobby) {
    this.authService.addHobby(hobby);
    this.loadHobbies();
  }

  deleteHobby(index: number) {
    this.authService.deleteHobby(index);
    this.loadHobbies();
  }
}
