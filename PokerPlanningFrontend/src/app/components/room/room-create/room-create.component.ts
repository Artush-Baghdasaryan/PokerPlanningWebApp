import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RoomScoreComponent } from '../room-score/room-score.component';
import { RoomService } from 'src/app/services/room.service';
import { GuestService } from 'src/app/services/guest.service';
import { RouterService } from 'src/app/services/router.service';
import { RoomHubService } from 'src/app/services/room-hub.service';

@Component({
  selector: 'app-room-create',
  templateUrl: './room-create.component.html',
  styleUrls: ['./room-create.component.scss']
})
export class RoomCreateComponent {
  public nameForm: FormGroup;

  constructor(
    private readonly roomService: RoomService,
    private readonly guestService: GuestService,
    private readonly routerService: RouterService,
    private readonly roomHub: RoomHubService
    ) {
    this.nameForm = new FormGroup({
      name: new FormControl(null, [Validators.required])
    })
  }

  public get nameControl(): string {
    return this.nameForm.controls["name"].value;
  }

  public onSubmit(): void {
    const roomName = this.nameControl;
    if (!roomName) {
      return;
    }

    this.roomService.createRoom(roomName).subscribe(room => {
      this.routerService.redirectToRoom(room.id);
    });
  }
}
