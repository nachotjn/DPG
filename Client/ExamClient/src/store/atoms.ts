import { atom } from "jotai";


export const playerBoardsAtom = atom<{ numbers: number[] }[]>([]);


export const winningNumbersAtom = atom<number[]>([]);

export interface Player {
    id: string;
    userName: string;
    email: string;
    phoneNumber: string;
    balance: number;
    isactive: boolean;
  }

  export const playerAtom = atom<Player | null>(null);


  export interface Game {
    gameid: string;
    weeknumber: number;
    year: number;
    winningnumbers: number[];
    iscomplete: boolean;
    prizesum: number;
  }  
  export const gameAtom = atom<Game | null>(null);
