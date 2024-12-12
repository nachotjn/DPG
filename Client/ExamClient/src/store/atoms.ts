import { atom } from "jotai";

// saldo del jugador
export const playerBalanceAtom = atom(0);

// mesas de los jugadores
export const playerBoardsAtom = atom<{ numbers: number[] }[]>([]);

// numeros ganadores
export const winningNumbersAtom = atom<number[]>([]);

// atoms.ts
export const selectedWeekAtom = atom<WeekData | null>(null);

export interface WeekData {
    weekNumber: number;
    players: number;
    boards: number;
    prize: number;
  }