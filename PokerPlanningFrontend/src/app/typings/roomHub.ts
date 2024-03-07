export enum HubEvents {
    GuestsUpdate = "GuestsUpdate",
    VoteReset = "VoteReset",
    Reveal = "Reveal",
    GuestVote = "GuestVote"
}

export enum HubCommand {
    GetConnectionId = "GetConnectionId",
    AddGuest = "AddGuest",
    GuestQuit = "GuestQuit",
    GuestVote = "GuestVote",
    Reveal = "Reveal",
    VoteReset = "VoteReset"
}