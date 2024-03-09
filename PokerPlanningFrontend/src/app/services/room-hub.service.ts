import { Injectable } from '@angular/core';
import { AppConfig } from '../app.config';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { HubCommand, HubEvents } from '../typings/roomHub';
import { Guest, GuestScore } from '../typings/guest';
import { GuestService } from './guest.service';

@Injectable({
  providedIn: 'root'
})
export class RoomHubService {
  
  private hubConnection: signalR.HubConnection;
  public guestsUpdated = new Subject<void>();
  public guestVoted = new Subject<GuestScore>();
  public reveal = new Subject<number>();
  public reset = new Subject<void>();

  constructor(private readonly appConfig: AppConfig) { 
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.appConfig.roomHub)
      .withAutomaticReconnect()
      .build();
  }

  public async startConnection(): Promise<void> {
    await this.hubConnection
      .start()
      .then(() => console.log("Connection started"))
      .catch(err => console.log("Error: ", err))
  }

  public addListeners(): void {
    this.hubConnection.on(HubEvents.GuestsUpdate, () => {
      this.guestsUpdated.next();
    });

    this.hubConnection.on(HubEvents.GuestVote, guest => {
      this.guestVoted.next({
        id: guest.id,
        score: guest.score
      });
    });

    this.hubConnection.on(HubEvents.Reveal, (score) => {
      this.reveal.next(score);
    });

    this.hubConnection.on(HubEvents.ResetVoting, () => {
      this.reset.next();
    })
  }

  public async addGuest(roomId: string): Promise<Guest> {
    return await this.hubConnection.invoke(HubCommand.AddGuest, roomId);
  }

  public async guestVote(guestId: string, roomId: string, score: number | null): Promise<Guest> {
    return await this.hubConnection.invoke(HubCommand.GuestVote, guestId, roomId, score);
  }

  public async revealVoting(roomId: string): Promise<void> {
    this.hubConnection.invoke(HubCommand.Reveal, roomId);
  }

  public resetVoting(roomId: string): void {
    this.hubConnection.invoke(HubCommand.ResetVoting, roomId);
  }

  public guestQuit(roomId: string): void {
    this.hubConnection.invoke(HubCommand.GuestQuit, roomId);
    this.hubConnection.stop();
  }

  public stopConnection(): void {
    this.hubConnection.stop();
  }

}
