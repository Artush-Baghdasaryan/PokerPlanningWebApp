import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomCreateComponent } from './components/room/room-create/room-create.component';
import { RoomGameComponent } from './components/room/room-game/room-game.component';
import { authGuard } from './services/auth.guard';

const routes: Routes = [
  { path: "", pathMatch: "full", redirectTo: "create-game"},
  {path: "create-game", component: RoomCreateComponent},
  {path: "game/:id", component: RoomGameComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
