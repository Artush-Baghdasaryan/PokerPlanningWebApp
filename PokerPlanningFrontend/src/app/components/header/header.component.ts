import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { GuestService } from 'src/app/services/guest.service';
import { RoomService } from 'src/app/services/room.service';
import { RoomInviteComponent } from '../room/room-invite/room-invite.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {

  @ViewChild(MatMenuTrigger) menuOpenBtn: MatMenuTrigger | null = null;

  public isMenuOpened: boolean = false;

  constructor(
    public roomService: RoomService,
    public guestService: GuestService,
    private dialog: MatDialog) {}

  public get guestName(): string {
    return this.guestService.getGuestName() ?? "";
  }

  public get roomName(): string | null {
    return this.roomService.getRoomName();
  }

  public isGuestCreated(): boolean {
    return this.guestService.getCurrentGuestId() !== null;
  }

  public menuTrigger(open: boolean): void {
    this.isMenuOpened = open;
  }

  public onExit(): void {
    this.guestService.logoutGuest();
  }

  public onInviteTeam(): void {
    this.dialog.open(RoomInviteComponent, {
      width: '583px',
      enterAnimationDuration: "300ms",
      exitAnimationDuration: "150ms"
    })
  }
  
}
