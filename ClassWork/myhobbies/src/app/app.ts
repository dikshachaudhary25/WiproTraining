import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu';
import { HomeComponent } from './home/home';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MenuComponent, RouterModule],
  templateUrl: './app.html'
})
export class AppComponent {
  selectedView: string = 'my';

  changeView(view: string) {
    this.selectedView = view;
  }
}