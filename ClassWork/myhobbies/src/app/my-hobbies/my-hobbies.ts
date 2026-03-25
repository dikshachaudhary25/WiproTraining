import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Hobby } from '../services/hobby';

@Component({
  selector: 'app-my-hobbies',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-hobbies.html'
})
export class MyHobbiesComponent {

  @Input() hobbies: Hobby[] = [];

  @Output() delete = new EventEmitter<number>();
  @Output() toggle = new EventEmitter<Hobby>();

  onDelete(id: number) {
    this.delete.emit(id);
  }

  onToggle(hobby: Hobby) {
    this.toggle.emit(hobby);
  }
}