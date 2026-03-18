import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  title = 'Login Page';
  username = '';
  password = '';
  myhobbies = ['Reading', 'Traveling', 'Cooking'];
  onLogin() {
    console.log('Login');
  }
}