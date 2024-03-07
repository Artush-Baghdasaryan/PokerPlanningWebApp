import { HtmlParser } from '@angular/compiler';
import { Component, EventEmitter, Output } from '@angular/core';
import { GuestService } from 'src/app/services/guest.service';
import { RoomHubService } from 'src/app/services/room-hub.service';
import { RoomService } from 'src/app/services/room.service';
import { GuestScore } from 'src/app/typings/guest';

@Component({
  selector: 'app-room-score',
  templateUrl: './room-score.component.html',
  styleUrls: ['./room-score.component.scss']
})
export class RoomScoreComponent {

  @Output() public scoreUpdated = new EventEmitter<GuestScore>();

  public pickedScoreId: string | null = null
  public scoreCards: Record<string, number> = {
    "0": 0,
    "1": 1,
    "2": 2,
    "3": 3,
    "5": 5,
    "8": 8,
    "13": 13,
    "21": 21,
    "34": 34,
    "55": 55,
    "89": 89,
    "?": 0,
    "coffee": 0
  };  


  public keys = Object.keys(this.scoreCards);

  constructor(
    private readonly guestService: GuestService,
    private readonly roomService: RoomService,
    private readonly roomHub: RoomHubService
  ) {
  }

  public onScore(event: MouseEvent): void {
    event.stopPropagation();

    const guestId = this.guestService.getCurrentGuestId();
    if (!guestId) return;
    const scoreElement = (event.target as HTMLElement).closest('.cards__item') as HTMLElement;
    this.pickedScoreId = scoreElement.getAttribute("id");
    const score = this.scoreCards[this.pickedScoreId!];
    const roomId = this.roomService.getRoomId();

    this.guestService.updateScore(guestId, score).subscribe(() => {
      this.roomHub.guestVote(guestId, roomId!, score);
      this.scoreUpdated.emit({
        id: guestId,
        score: score
      });
    });
  }

}
