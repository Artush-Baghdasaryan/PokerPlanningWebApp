import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';
import { Room } from '../typings/room';
import { AppConfig } from '../app.config';
import { Guest, GuestScore } from '../typings/guest';
import { GuestService } from './guest.service';
import { RoomHubService } from './room-hub.service';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  private api: string = this.config.apiUrl;
  private controller: string = "Room";
  private roomNameKey: string = "roomName";
  private roomIdKey: string = "roomId";

  constructor(
    private http: HttpClient,
    private config: AppConfig,
    private readonly roomHubService: RoomHubService) { }

  public getRoomById(roomId: string): Observable<Room> {
    return this.http.get<Room>(`${this.api}/${this.controller}/${roomId}`).pipe(tap({
      next: result => {
        this.saveRoomData(result);
      },
      error: _ => {
        //to do
      }
    }))
  }

  public createRoom(roomName: string): Observable<Room> {
    return this.http.post<Room>(`${this.api}/${this.controller}/create-new-room?&roomName=${roomName}`, null).pipe(tap({
      next: result => {
        this.saveRoomData(result);
      },
      error: _ => {
        //to do
      }
    }))
  }

  public removeRoom(roomId: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/${this.controller}?roomId:${roomId}`).pipe(
      tap({
        next: () => {
          this.cleanRoomData();
        }
      })
    )
  }

  public getRoomGuests(roomId: string): Observable<Guest[]> {
    return this.http.get<Guest[]>(`${this.api}/${this.controller}/get-guests/${roomId}`).pipe();
  }

  public addGuestToRoom(roomId: string): Observable<Guest> {
    return this.http.post<Guest>(`${this.api}/${this.controller}/add-guest/${roomId}`, null).pipe(
      tap({
        next: guest => {
          // to do
        }
      })
    );
  }

  public cleanRoomData(): void {
    window.localStorage.removeItem(this.roomIdKey);
    window.localStorage.removeItem(this.roomNameKey);
  }

  public saveRoomData(room: Room): void {
    window.localStorage.setItem(this.roomIdKey, room.id);
    window.localStorage.setItem(this.roomNameKey, room.name);
  }

  public getRoomName(): string | null {
    return window.localStorage.getItem(this.roomNameKey);
  }

  public getRoomId(): string | null {
    return window.localStorage.getItem(this.roomIdKey);
  }

}
