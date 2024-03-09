import { ActivatedRoute, CanActivateFn, Router } from "@angular/router";
import { GuestService } from "./guest.service";
import { inject } from "@angular/core";
import { RoomScoreComponent } from "../components/room/room-score/room-score.component";
import { RoomService } from "./room.service";
import { RouterService } from "./router.service";

export const authGuard: CanActivateFn = (route, state) => {
    const guestService = inject(GuestService);
    const roomService = inject(RoomService);
    const router = inject(ActivatedRoute);
    const routerService = inject(RouterService);

    const guestId = guestService.getCurrentGuestId();
    if (guestId === null) {
        const roomId = window.location.href.split('/').reverse()[0];
        if (!roomId) {
            routerService.redirectToCreateGame();
            return false;
        }

        // guestService.registerGuest(roomId).subscribe();
    }

    return true;
  };
  