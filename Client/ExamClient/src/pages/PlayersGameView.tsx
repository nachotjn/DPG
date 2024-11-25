import { useState } from "react";
import "./PlayersGameView.css";

const PlayersGameView = () => {
  const [selectedNumbers, setSelectedNumbers] = useState<Set<number>>(new Set());
  const [money, setMoney] = useState(0);
  const [savedBoards, setSavedBoards] = useState<number[][]>([]);

  const handleNumberSelect = (number: number) => {
    setSelectedNumbers((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(number)) {
        newSet.delete(number);
        setMoney(money + 10); 
      } else {
        newSet.add(number);
        setMoney(money - 10);
      }
      return newSet;
    });
  };

  const handleSaveBoard = () => {
    setSavedBoards((prev) => [...prev, Array.from(selectedNumbers)]);
  };

  const handlePlay = () => {
    console.log("Playing with numbers:", Array.from(selectedNumbers));
  };

  const renderGrid = () => {
    let numbers = [];
    for (let i = 1; i <= 16; i++) {
      numbers.push(i);
    }

    return numbers.map((number) => (
      <div
        key={number}
        className={`number-box ${selectedNumbers.has(number) ? "selected" : ""}`}
        onClick={() => handleNumberSelect(number)}
      >
        {number}
      </div>
    ));
  };

  return (
    <div className="players-game-container">
      <div className="game-header">
      <img src="./src/assets/images/logo.png" alt="Logo" className="logo" />
        <h1>DEAD PIGEONS</h1>
        <p>Balance: {money} DKK</p>
      </div>

      <div className="game-body">
        <div className="number-grid">
          {renderGrid()}
        </div>

        <div className="game-options">
          <button onClick={handlePlay}>Play</button>
          <button onClick={handleSaveBoard}>Save Board</button>

          <div className="saved-boards">
            <h3>Saved Boards</h3>
            {savedBoards.length > 0 && (
              <ul>
                {savedBoards.map((board, index) => (
                  <li key={index}>{board.join(", ")}</li>
                ))}
              </ul>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default PlayersGameView;
