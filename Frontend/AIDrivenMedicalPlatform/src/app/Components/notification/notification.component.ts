import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { INotification } from '../../Core/Interfaces/inotification';
import { NotificationService } from '../../Core/Services/notification.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-notification',
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.scss'
})
export class NotificationComponent implements OnInit{
  Notifications : INotification[] = [];
  
  notificationService = inject(NotificationService);
  ngOnInit(): void {
    this.notificationService.getPatientNotifications().subscribe((res) => {
      // console.log(res);
      this.Notifications = res;
      // console.log(this.Notifications);
    });
  }

  getNotificationType(type: number): string {
    switch (type) {
      case 0: return 'success';
      case 1: return 'alert';
      case 2: return 'reminder';
      default: return 'info';
    }
  }

  getNotificationIconClass(type: number): string {
    const notificationType = this.getNotificationType(type);
    
    switch (notificationType) {
      case 'success':
        return 'bg-green-100';
      case 'alert':
        return 'bg-red-100';
      case 'reminder':
        return 'bg-blue-100';
      default:
        return 'bg-gray-100';
    }
  }

    getNotificationIconColor(type: number): string {
    const notificationType = this.getNotificationType(type);
    
    switch (notificationType) {
      case 'success':
        return 'text-green-600';
      case 'alert':
        return 'text-red-600';
      case 'reminder':
        return 'text-blue-600';
      default:
        return 'text-gray-600';
    }
  }

    getNotificationBadgeClass(type: number): string {
    const notificationType = this.getNotificationType(type);
    
    switch (notificationType) {
      case 'success':
        return 'bg-green-100 text-green-800';
      case 'alert':
        return 'bg-red-100 text-red-800';
      case 'reminder':
        return 'bg-blue-100 text-blue-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  }

  getNotificationTypeLabel(type: number): string {
    if (typeof type === 'string') {
      return type;
    }
    
    switch (type) {
      case 0: return 'Success';
      case 1: return 'Alert';
      case 2: return 'Reminder';
      default: return 'Info';
    }
  }


  
  formatDate(dateString: string): string {
    const date = new Date(dateString);
    
    const now = new Date();
    const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

    // Debug log to see what's happening
    console.log('Original string:', dateString);
    console.log('Parsed date:', date);
    console.log('Current time:', now);
    console.log('Difference in seconds:', diffInSeconds);

    if (diffInSeconds < 0) {
      // Future date
      return 'Just now';
    } else if (diffInSeconds < 60) {
      return 'Just now';
    } else if (diffInSeconds < 3600) {
      const minutes = Math.floor(diffInSeconds / 60);
      return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    } else if (diffInSeconds < 86400) {
      const hours = Math.floor(diffInSeconds / 3600);
      return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    } else {
      return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    }
  }

  toastr = inject(ToastrService);
  RemoveNotification(NotificationId : number){
    this.notificationService.removeNotification(NotificationId).subscribe({
      next: (res) => {
        this.toastr.success(res.message);
        this.Notifications = this.Notifications.filter(n => n.id !== NotificationId);
      },
      error: (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    });
  }
}
