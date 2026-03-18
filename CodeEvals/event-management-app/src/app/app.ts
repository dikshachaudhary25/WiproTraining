import { Component } from '@angular/core';
import { EventListComponent } from './event-list/event-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [EventListComponent],
  templateUrl: './app.html'
})
export class AppComponent {}