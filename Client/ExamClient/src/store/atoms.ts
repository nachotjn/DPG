import { atom } from "jotai";

// saldo del jugador
export const playerBalanceAtom = atom(0);

// mesas de los jugadores
export const playerBoardsAtom = atom<{ numbers: number[] }[]>([]);

// numeros ganadores
export const winningNumbersAtom = atom<number[]>([]);
