export interface Board {
  numbers: number[];
}

export interface Player {
  id: string;
  name: string;
  isActive: boolean;
  balance: number;
  boards: Board[];
}
