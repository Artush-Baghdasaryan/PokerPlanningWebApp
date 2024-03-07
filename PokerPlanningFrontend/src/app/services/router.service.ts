import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable({
    providedIn: 'root'
})
export class RouterService {
    
    constructor(private readonly router: Router) {}

    public redirectToRoom(roomId: string) {
        this.router.navigate(["/game", roomId]);
    }

    public redirectToCreateGame() {
        this.router.navigate(["/create-game"]);
    }
}