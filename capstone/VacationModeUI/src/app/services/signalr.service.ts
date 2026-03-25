import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public hubConnection!: signalR.HubConnection;

  startConnection(token: string) {
    
    
    
    
    if (this.hubConnection && this.hubConnection.state !== signalR.HubConnectionState.Disconnected) {
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      
      .withUrl('http://localhost:5127/chatHub', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR successfully connected'))
      .catch(err => console.error('SignalR connection failed: ', err));
  }

  onMessageReceived(callback: (senderId: string, message: any) => void) {
    if (!this.hubConnection) return;
    this.hubConnection.on('ReceiveMessage', callback);
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop().catch(err => console.error('Failed to stop SignalR: ', err));
    }
  }
}
