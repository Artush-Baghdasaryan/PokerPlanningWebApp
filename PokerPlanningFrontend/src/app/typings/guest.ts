export interface Guest {
    id: string,
    index: number,
    score: number | null,
    connectionId: string | null
}

export interface GuestScore {
    id: string,
    score: number | null
}