import { Component, HostListener } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';
import { NotificationService, AppNotification } from '../../services/notification';
import { SignalRService } from '../../services/signalr.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  notifications: AppNotification[] = [];
  unreadCount = 0;
  showDropdown = false;
  isMenuOpen = false;
  isMarkingAllAsRead = false;

  constructor(
    public auth: AuthService,
    private router: Router,
    private notificationService: NotificationService,
    private signalRService: SignalRService
  ) {
    this.loadNotifications();
    this.setupRealTimeNotifications();
  }

  loadNotifications() {
    if (this.auth.isLoggedIn()) {
      this.notificationService.getMyNotifications().subscribe(data => {
        this.notifications = data;
        this.unreadCount = data.filter(n => !n.isRead).length;
      });
    }
  }

  setupRealTimeNotifications() {
    const token = this.auth.getToken();
    if (token) {
      
      this.signalRService.startConnection(token);
      
      
      if (this.signalRService.hubConnection) {
        this.signalRService.hubConnection.on('ReceiveNotification', (newNotif: AppNotification) => {
          this.notifications.unshift(newNotif); 
          this.unreadCount++;
        });
      }
    }
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
    
    if (this.isMenuOpen) this.showDropdown = false;
  }

  closeMenu() {
    this.isMenuOpen = false;
  }

  toggleNotifications() {
    this.showDropdown = !this.showDropdown;
    if (this.showDropdown) {
      this.closeMenu();
      this.loadNotifications();
    }
  }

  handleNotificationClick(notif: AppNotification, event: Event) {
    event.stopPropagation();
    
    if (!notif.isRead) {
      this.markAsRead(notif);
    }

    this.navigateToTarget(notif);
    
    
    this.showDropdown = false;
  }

  navigateToTarget(notification: AppNotification) {
    switch (notification.type) {
      case 'Message':
        
        this.router.navigate(['/messages'], {
          queryParams: { propertyId: notification.referenceId }
        });
        break;
      case 'Booking':
        this.router.navigate(['/my-reservations']);
        break;
      case 'Property':
        this.router.navigate(['/properties', notification.referenceId]);
        break;
      default:
        
        this.router.navigate(['/']);
    }
  }

  markAsRead(notif: AppNotification) {
    if (notif.isRead) return;
    
    
    notif.isRead = true;
    this.unreadCount = Math.max(0, this.unreadCount - 1);
    
    this.notificationService.markAsRead(notif.notificationId).subscribe({
      error: () => {
        
        notif.isRead = false;
        this.unreadCount++;
      }
    });
  }

  markAllAsRead() {
    if (this.unreadCount === 0 || this.isMarkingAllAsRead) return;
    
    this.isMarkingAllAsRead = true;
    
    this.notificationService.markAllAsRead().subscribe({
      next: () => {
        this.notifications.forEach(n => n.isRead = true);
        this.unreadCount = 0;
        this.isMarkingAllAsRead = false;
      },
      error: () => {
        this.isMarkingAllAsRead = false;
      }
    });
  }

  logout() {
    this.auth.logout();
    this.notifications = [];
    this.unreadCount = 0;
    this.showDropdown = false;
    this.isMenuOpen = false;
    this.signalRService.stopConnection();
    this.router.navigate(['/login']);
  }

  
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.navbar')) {
      this.showDropdown = false;
      this.isMenuOpen = false;
    }
  }
}
