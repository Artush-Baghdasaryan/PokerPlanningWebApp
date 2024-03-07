import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AppConfig } from '../app.config';
import { Guest } from '../typings/guest';
import { RouterService } from './router.service';
import { RoomService } from './room.service';
import { RoomHubService } from './room-hub.service';
import { ActivatedRoute } from '@angular/router';
import { HubCommand } from '../typings/roomHub';

@Injectable({
  providedIn: 'root'
})
export class GuestService {
  private api: string = this.appConfig.apiUrl;
  private controller: string = "Guest";
  private guestNameKey: string = "name";
  private guestIdKey: string = "guestId";

  constructor(
    private httpClient: HttpClient,
    private appConfig: AppConfig,
    private readonly routerService: RouterService,
    private readonly roomService: RoomService,
    private readonly roomHub: RoomHubService,
    ) { }

  public deleteGuest(guestId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.api}/${this.controller}/${guestId}`).pipe(
      tap({
        next: () => {
          // to do
        }
      })
    );
  }

  public updateScore(guestId: string, score: number): Observable<void> {
    return this.httpClient.post<void>(`${this.api}/${this.controller}/score/${guestId}?score=${score}`, null).pipe();
  }

  public saveGuestData(guest: Guest): void {
    this.logoutGuest();
    window.localStorage.setItem(this.guestNameKey, `Guest ${guest.index}`);
    window.localStorage.setItem(this.guestIdKey, guest.id);
  }

  public logoutGuest(): void {
    const guestId = this.getCurrentGuestId();
    if (!guestId) return;

    this.deleteGuest(guestId).subscribe();
    window.localStorage.removeItem(this.guestIdKey);
    window.localStorage.removeItem(this.guestNameKey);
    this.roomService.cleanRoomData();
    this.routerService.redirectToCreateGame();
    this.roomHub.stopConnection();
  }

  public getCurrentGuestId(): string | null {
    return window.localStorage.getItem(this.guestIdKey);
  }

  public getGuestName(): string | null {
    return window.localStorage.getItem(this.guestNameKey);
  }

}
