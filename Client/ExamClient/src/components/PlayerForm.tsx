import React, { useState } from "react";
import { useAtom } from "jotai";
import { playerBoardsAtom } from "../store/atoms";

const PlayerForm = () => {
  const [playerBoards, setPlayerBoards] = useAtom(playerBoardsAtom);
  const [numbers, setNumbers] = useState<number[]>([]);

  const handleBoardSelection = (selectedNumbers: number[]) => {
    setNumbers(selectedNumbers);
    // logica para actualizar el estado de los tableros
  };

  const handleSubmit = () => {
    // logica para enviar el formulario y agregar el tablero a la lista
  };

  return (
    <div>
      <h2>Choose Numbers</h2>
      {/* agregar logica de seleccion de numeros */}
      <button onClick={handleSubmit}>Submit</button>
    </div>
  );
};

export default PlayerForm;
