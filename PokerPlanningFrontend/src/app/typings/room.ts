import { Guest } from "./guest";

export interface Room {
    id: string,
    name: string,
    admin: Guest,
    guests: Guest[]
}

