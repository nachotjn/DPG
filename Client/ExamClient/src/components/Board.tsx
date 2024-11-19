import React, { useState } from "react";
import { atom, useAtom } from "jotai";
import { createBoard } from "../services/api"; // importa la funcion para enviar datos al backend

// definir un atomo para almacenar los numeros seleccionados del tablero
export const selectedNumbersAtom = atom<number[]>([]);

// definir el atomo para manejar el costo del tablero basado en la cantidad de numeros seleccionados
export const boardCostAtom = atom((get) => {
  const selectedNumbers = get(selectedNumbersAtom);
  switch (selectedNumbers.length) {
    case 5:
      return 20; // 5 numeros cuestan 20 DKK
    case 6:
      return 40; // 6 numeros cuestan 40 DKK
    case 7:
      return 80; // 7 numeros cuestan 80 DKK
    case 8:
      return 160; // 8 numeros cuestan 160 DKK
    default:
      return 0; // no se ha seleccionado ningun numero
  }
});

const Board: React.FC = () => {
  const [selectedNumbers, setSelectedNumbers] = useAtom(selectedNumbersAtom);
  const [boardCost] = useAtom(boardCostAtom);

  // agregar un numero al tablero
  const selectNumber = (number: number) => {
    if (selectedNumbers.length < 8 && !selectedNumbers.includes(number)) {
      setSelectedNumbers([...selectedNumbers, number]);
    }
  };

  // quitar un numero del tablero
  const removeNumber = (number: number) => {
    setSelectedNumbers(selectedNumbers.filter((n) => n !== number));
  };

  return (
    <div>
      <h2>Board</h2>
      <div>
        {/* muestra los numeros seleccionados */}
        <p>Selected numbers: {selectedNumbers.join(", ")}</p>
      </div>

      <div>
        {/* muestra botones para seleccionar numeros del 1 al 16 */}
        {[...Array(16).keys()].map((i) => (
          <button
            key={i + 1}
            onClick={() => {
              selectedNumbers.includes(i + 1)
                ? removeNumber(i + 1)
                : selectNumber(i + 1);
            }}
            style={{
              backgroundColor: selectedNumbers.includes(i + 1)
                ? "green"
                : "lightgray",
            }}
          >
            {i + 1}
          </button>
        ))}
      </div>

      <div>
        {/* muestra el costo del tablero */}
        <p>Board cost: {boardCost} DKK</p>
      </div>
    </div>
  );
};

export default Board;
