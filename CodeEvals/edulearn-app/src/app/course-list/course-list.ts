import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseDetailComponent } from '../course-detail/course-detail';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, CourseDetailComponent],
  templateUrl: './course-list.html'
})
export class CourseListComponent {
  courses = [
    { id: 1, title: 'Angular Basics', duration: '4 weeks' },
    { id: 2, title: 'React Mastery', duration: '6 weeks' },
    { id: 3, title: 'Node.js Advanced', duration: '5 weeks' }
  ];

  selectedCourse: any = null;

  selectCourse(course: any) {
    this.selectedCourse = course;
  }
}