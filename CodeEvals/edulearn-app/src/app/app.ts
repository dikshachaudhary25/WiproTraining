import { Component } from '@angular/core';
import { CourseListComponent } from './course-list/course-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CourseListComponent],
  templateUrl: './app.html'
})
export class AppComponent {}