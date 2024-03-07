import {Injectable} from "@angular/core";

@Injectable(
  {'providedIn': 'root'}
)
export class AppConfig {
  public get apiRoot(): string { return "/api"; }
  public get apiUrl(): string { return "http://localhost:5135"; }
  public get roomHub(): string { return "http://localhost:5135/roomHub"; }
}