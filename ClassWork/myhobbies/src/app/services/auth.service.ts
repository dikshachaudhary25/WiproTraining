import { Injectable } from '@angular/core';

export interface Hobby {
  name: string;
  fav: boolean;
}

export interface User {
  username: string;
  password: string;
  hobbies: Hobby[];
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly USERS_KEY = 'app_users';
  private readonly CURRENT_USER_KEY = 'app_current_user';

  /** Tracks which hobby view is active: 'my' | 'add' | 'fav' */
  selectedView = 'my';

  // ─── User persistence ────────────────────────────────────────────────────

  private getUsers(): User[] {
    const raw = localStorage.getItem(this.USERS_KEY);
    return raw ? JSON.parse(raw) : [];
  }

  private saveUsers(users: User[]): void {
    localStorage.setItem(this.USERS_KEY, JSON.stringify(users));
  }

  // ─── Auth ────────────────────────────────────────────────────────────────

  register(username: string, password: string): boolean {
    const users = this.getUsers();
    if (users.find(u => u.username === username)) {
      return false; // username already taken
    }
    users.push({ username, password, hobbies: [] });
    this.saveUsers(users);
    return true;
  }

  login(username: string, password: string): boolean {
    const users = this.getUsers();
    const match = users.find(u => u.username === username && u.password === password);
    if (match) {
      localStorage.setItem(this.CURRENT_USER_KEY, username);
      this.selectedView = 'my';
      return true;
    }
    return false;
  }

  logout(): void {
    localStorage.removeItem(this.CURRENT_USER_KEY);
    this.selectedView = 'my';
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem(this.CURRENT_USER_KEY);
  }

  getCurrentUser(): User | null {
    const username = localStorage.getItem(this.CURRENT_USER_KEY);
    if (!username) return null;
    return this.getUsers().find(u => u.username === username) ?? null;
  }

  // ─── View ────────────────────────────────────────────────────────────────

  setView(view: string): void {
    this.selectedView = view;
  }

  // ─── Hobbies ─────────────────────────────────────────────────────────────

  getHobbies(): Hobby[] {
    return this.getCurrentUser()?.hobbies ?? [];
  }

  addHobby(hobby: Hobby): void {
    const users = this.getUsers();
    const user = users.find(u => u.username === localStorage.getItem(this.CURRENT_USER_KEY));
    if (user) {
      user.hobbies.push(hobby);
      this.saveUsers(users);
    }
  }

  deleteHobby(index: number): void {
    const users = this.getUsers();
    const user = users.find(u => u.username === localStorage.getItem(this.CURRENT_USER_KEY));
    if (user) {
      user.hobbies.splice(index, 1);
      this.saveUsers(users);
    }
  }
}
