import React from "react";
import { useAtom } from "jotai";
import { winningNumbersAtom, playerBoardsAtom } from "../store/atoms";

const GameOverview = () => {
  const [winningNumbers] = useAtom(winningNumbersAtom);
  const [playerBoards] = useAtom(playerBoardsAtom);

  return (
    <div>
      <h2>Game Overview</h2>
      <h3>Winning Numbers: {winningNumbers.join(", ")}</h3>
      <div>
        <h4>Player Boards</h4>
        {playerBoards.map((board, index) => (
          <div key={index}>
            <p>
              Board {index + 1}: {board.numbers.join(", ")}
            </p>
            {/* aqui se compara si el tablero tiene los numeros ganadores */}
          </div>
        ))}
      </div>
    </div>
  );
};

export default GameOverview;
