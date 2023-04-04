import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection?: signalR.HubConnection;
  private isConnectionStarted = new BehaviorSubject<boolean>(false);
  token?:string | null;
  email?:string;
  constructor() {this.token=localStorage.getItem('jwt'); }

  startConnection() {
    if(this.token)
    {
        const payload = jwt_decode(this.token);
        this.email=(payload as any).email;
        this.hubConnection = new signalR.HubConnectionBuilder()
          .withUrl(`https://localhost:7149/scientistHub?userId=${this.email}`, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
          })
          .build();
        this.hubConnection.start()
          .then(() => {
            console.log('SignalR connection started');
            this.isConnectionStarted.next(true);
          })
          .catch(err => console.log(`Error while starting SignalR connection: ${err}`));
        }
        return this.hubConnection;
  }

  stopConnection(): void {
    this.hubConnection?.stop();
    this.isConnectionStarted.next(false);
  }

  on(eventName: string, callback: (...args: any[]) => void): void {
    this.hubConnection?.on(eventName, callback);
  }

  off(eventName: string, callback: (...args: any[]) => void): void {
    this.hubConnection?.off(eventName, callback);
  }

  getIsConnectionStarted(): BehaviorSubject<boolean> {
    return this.isConnectionStarted;
  }
}
