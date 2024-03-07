import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GuestService } from 'src/app/services/guest.service';
import { RoomHubService } from 'src/app/services/room-hub.service';
import { RoomService } from 'src/app/services/room.service';
import { Guest, GuestScore } from 'src/app/typings/guest';
import { Room } from 'src/app/typings/room';

@Component({
  selector: 'app-room-game',
  templateUrl: './room-game.component.html',
  styleUrls: ['./room-game.component.scss']
})
export class RoomGameComponent implements OnInit {

  @ViewChild("gameTable") table: ElementRef | null = null;

  public guests: Guest[] | null = null;
  public roomId: string | null = null;

  constructor(
    private readonly roomService: RoomService,
    private readonly guestService: GuestService,
    private readonly route: ActivatedRoute,
    private readonly roomHub: RoomHubService
  ) {}

  public get tableElement(): HTMLElement {
    return this.table?.nativeElement;
  }

  public ngOnInit(): void {
    this.roomId = this.route.snapshot.paramMap.get("id");
    if (this.roomId) this.roomService.getRoomById(this.roomId).subscribe();
    
    this.roomHub.startConnection().then(() => {
      this.roomHub.addListeners();
      this.roomGuardCheck();
    });

    
    this.roomHub.guestsUpdated.subscribe((): void => {
      this.getGuests();
    });

    this.roomHub.guestVoted.subscribe((guestScore: GuestScore) => {
      this.onGuestVote(guestScore);
    });


  }

  public async getGuests(): Promise<void> {
    if (!this.roomId) {
      return;
    }

    await this.roomService.getRoomGuests(this.roomId).subscribe(guests => {
      this.guests = guests;
      setTimeout(() => {
        this.renderGuestsCards();
      });
    });
    
  }

  public renderGuestsCards(): void {
    this.guests?.forEach(guest => {
      this.renderGuestCard(guest);
    });

    console.log(this.guests);
  }

  private renderGuestCard(guest: Guest): void {
    console.log("table", this.tableElement);
    const card = this.tableElement.querySelector(`[id='${guest.id}']`) as HTMLElement;
    const position = positions[guest.index];
    
    card.style.top = `${position.top}px`;
    card.style.bottom = `${position.bottom}px`;
    card.style.left = `${position.left}px`;
    card.style.right = `${position.right}px`;
  }

  public onGuestVote(guestScore: GuestScore): void {
    const guest = this.guests?.find(g => g.id === guestScore.id);
    if (!guest) return;

    const guestCardElem = this.tableElement.querySelector(`[id='${guest.id}'] .card`);
    guestCardElem?.classList.add("voted");
  }

  public async roomGuardCheck(): Promise<void> {
    const guestId = this.guestService.getCurrentGuestId();
    if (guestId) {
      this.getGuests();
      return;
    }

    this.roomHub.addGuest(this.roomId!).then(guest => {
      this.guestService.saveGuestData(guest);
    });
  }
}


interface Position {
  top: number | null;
  left: number | null;
  right: number | null;
  bottom: number | null;
}

const positions: { [index: number]: Position } = {
  1: { top: -125, left: null, right: 75, bottom: null},
  2: { top: 100, left: null, right: -100, bottom: null},
  3: { top: null, left: null, right: 75, bottom: -125},
  4: { top: null, left: null, right: 335, bottom: -125},
  5: { top: null, left: 75, right: null, bottom: -125},
  6: { top: 100, left: -100, right: null, bottom: null},
  7: { top: -125, left: 75, right: null, bottom: null},
  8: { top: -125, left: null, right: 335, bottom: null},
}
