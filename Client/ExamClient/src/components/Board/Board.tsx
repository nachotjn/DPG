import React from "react";
import { atom, useAtom } from "jotai";
import "./board.module.css"; 

export const selectedNumbersAtom = atom<number[]>([]);

export const boardCostAtom = atom((get) => {
  const selectedNumbers = get(selectedNumbersAtom);
  switch (selectedNumbers.length) {
    case 5:
      return 20; // 5 números cuestan 20 DKK
    case 6:
      return 40; // 6 números cuestan 40 DKK
    case 7:
      return 80; // 7 números cuestan 80 DKK
    case 8:
      return 160; // 8 números cuestan 160 DKK
    default:
      return 0; // No se ha seleccionado ningún número
  }
});

const Board: React.FC = () => {
  const [selectedNumbers, setSelectedNumbers] = useAtom(selectedNumbersAtom);
  const [boardCost] = useAtom(boardCostAtom);

  // Función para manejar los clics y agregar o quitar números del tablero
  const handleClick = (number: number) => {
    if (selectedNumbers.includes(number)) {
      // Si ya está seleccionado, eliminarlo
      setSelectedNumbers(selectedNumbers.filter((n) => n !== number));
    } else {
      // Si no está seleccionado, agregarlo
      if (selectedNumbers.length < 8) {
        setSelectedNumbers([...selectedNumbers, number]);
      }
    }
  };

  return (
    <div className="board-container">
      {/* Contenedor que agrupa la información y el tablero */}
      <div className="board-wrapper">
        {/* Información lateral izquierda */}
        <div className="side-info">
          <div className="board-info">
            <h1 className="game-title">Player</h1>
            <p className="game-info">Selected numbers: {selectedNumbers.join(", ")}</p>
            <p className="game-info">Board cost: {boardCost} DKK</p>
            <p className="game-info">Your Balance: 0 DKK</p>
          </div>
          <div>
            <h2 className="game-title">Game Overview</h2>
            <p className="game-info">Numbers selected: {selectedNumbers.length}</p>
          </div>
        </div>

        {/* Tablero 4x4 */}
        <div className="board">
          {/* Genera los botones del tablero (4x4 = 16 botones) */}
          {[...Array(16).keys()].map((i) => (
            <button
              key={i + 1}
              className={`board-button ${selectedNumbers.includes(i + 1) ? "selected" : ""}`}
              onClick={() => handleClick(i + 1)}
              style={{
                backgroundColor: selectedNumbers.includes(i + 1) ? "" : "",
              }}
            >
              {i + 1}
            </button>
          ))}
        </div>
      </div>
      <button className="play-button">PLAY</button>
    </div>
  );
};

export default Board;
