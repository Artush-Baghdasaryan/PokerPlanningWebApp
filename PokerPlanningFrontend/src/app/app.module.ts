import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { RoomCreateComponent } from './components/room/room-create/room-create.component';
import { RoomScoreComponent } from './components/room/room-score/room-score.component';
import { HttpClientModule } from "@angular/common/http";
import { authInterceptorProviders } from './services/interceptor/auth.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { RoomGameComponent } from './components/room/room-game/room-game.component';
import {MatIconModule} from '@angular/material/icon';
import {MatRippleModule} from '@angular/material/core';
import {MatDialogModule} from '@angular/material/dialog';
import { RoomInviteComponent } from './components/room/room-invite/room-invite.component';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    RoomCreateComponent,
    RoomScoreComponent,
    RoomGameComponent,
    RoomInviteComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatMenuModule,
    MatIconModule,
    MatRippleModule,
    MatDialogModule
  ],
  providers: [authInterceptorProviders],
  bootstrap: [AppComponent]
})
export class AppModule { }
