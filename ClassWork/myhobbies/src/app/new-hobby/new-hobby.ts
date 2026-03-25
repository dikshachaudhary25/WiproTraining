import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Hobby } from '../services/hobby';

@Component({
  selector: 'app-new-hobby',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './new-hobby.html'
})
export class NewHobbyComponent {

  name = '';
  fav = false;

  @Output() add = new EventEmitter<Hobby>();

  submit() {
    if (!this.name.trim()) return;

    this.add.emit({
      name: this.name,
      fav: this.fav
    });

    this.name = '';
    this.fav = false;
  }
}