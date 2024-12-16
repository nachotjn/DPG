import React, { useState } from "react";
import { atom, useAtom } from "jotai";
import { gameAtom, playerAtom } from "../../store/atoms"; // Import gameAtom and playerAtom
import { createBoard } from "../../services/api"; // Import API method to create a board
import "./board.module.css";

// Atom for selected numbers
export const selectedNumbersAtom = atom<number[]>([]);

// Atom for calculating board cost
export const boardCostAtom = atom((get) => {
  const selectedNumbers = get(selectedNumbersAtom);
  switch (selectedNumbers.length) {
    case 5:
      return 20; // 5 numbers cost 20 DKK
    case 6:
      return 40; // 6 numbers cost 40 DKK
    case 7:
      return 80; // 7 numbers cost 80 DKK
    case 8:
      return 160; // 8 numbers cost 160 DKK
    default:
      return 0; // No numbers selected
  }
});

const Board: React.FC = () => {
  const [selectedNumbers, setSelectedNumbers] = useAtom(selectedNumbersAtom);
  const [boardCost] = useAtom(boardCostAtom);
  const [game, setGame] = useAtom(gameAtom); 
  const [player, setPlayer] = useAtom(playerAtom); 
  
  const [isAutoplay, setIsAutoplay] = useState(false);
  const [autoplayWeeks, setAutoplayWeeks] = useState(0); 
  
  if (!game) {
    return <div>Loading game...</div>;
  }

  const handlePlay = async () => {
    const now = new Date();
  
    const danishOffset = 1; 
    const danishTime = new Date(now.getTime() + danishOffset * 60 * 60 * 1000);
  
    const day = danishTime.getDay(); 
    const hours = danishTime.getHours();
  
    if (day === 6 && hours >= 17) { 
      alert("You can no longer join the game this week. The cutoff is 5 PM Saturday.");
      return;
    }
  
    if (game.iscomplete === true) {
      alert("This game is already complete, please wait for next week's game");
      return;
    }
  
    if (player && player.balance >= boardCost && game.iscomplete === false) {
      const totalCost = isAutoplay ? boardCost * autoplayWeeks : boardCost;
      const newBalance = player.balance - totalCost;
  
      if (newBalance < 0) {
        alert("Insufficient balance to play for the selected weeks of autoplay.");
        return;
      }
  
      setPlayer({ ...player, balance: newBalance });
  
      // Prepare data to create board
      const boardData = {
        numbers: selectedNumbers,
        isAutoplay,
        autoplayWeeks,
        playerId: player.id,
        gameId: game.gameid,
      };
  
      try {
        const response = await createBoard(boardData);
        if (response) { 
          alert(`Board created successfully! Remaining balance: ${newBalance} DKK`);
        } else {
          alert("Board creation failed. Please try again.");
        }
      } catch (error) {
        console.error("Error creating board", error);
        alert("There was an issue creating your board.");
      }
    } else {
      alert("Insufficient balance to play.");
    }
  };
  
  
  

  const handleClick = (number: number) => {
    if (selectedNumbers.includes(number)) {
      setSelectedNumbers(selectedNumbers.filter((n) => n !== number));
    } else {
      if (selectedNumbers.length < 8) {
        setSelectedNumbers([...selectedNumbers, number]);
      }
    }
  };

  const renderGameStatus = (isComplete: boolean) => {
    return isComplete ? 'Completed' : 'In progress';
  };
  const playerName = player?.userName || "Please log in to pay";  

  return (
    <div className="board-container">
      {/* Board container */}
      <div className="board-wrapper">
        {/* Left-side info */}
        <div className="side-info">
          <div className="board-info">
            <h1 className="game-title">Player: {playerName.replace(/_/g, ' ')}</h1>
            <p className="game-info">Selected numbers: {selectedNumbers.join(", ")}</p>
            <p className="game-info">Board cost: {boardCost} Kr.</p>
            <p className="game-info">Your Balance: {player?.balance || 0} Kr.</p>
          </div>
          <div>
            <h2 className="game-title">Game Overview</h2>
            {/* Game information */}
            <p className="game-info">Game Week: {game.weeknumber}</p>
            <p className="game-info">Game Year: {game.year}</p>
            <p className="game-info">Prize sum {game.prizesum} Kr.</p>
            <p className="game-info">Status: {renderGameStatus(game.iscomplete)}</p>
            <p className="game-info">Numbers selected: {selectedNumbers.length}</p>
          </div>
        </div>

        {/* 4x4 Board */}
        <div className="board">
          {[...Array(16).keys()].map((i) => (
            <button
              key={i + 1}
              className={`board-button ${selectedNumbers.includes(i + 1) ? "selected" : ""}`}
              onClick={() => handleClick(i + 1)}
              style={{
                backgroundColor: selectedNumbers.includes(i + 1) ? "lightblue" : "lightgray",
              }}
            >
              {i + 1}
            </button>
          ))}
        </div>
      </div>

      {/* Autoplay Checkbox and Weeks Input */}
      <div className="autoplay-section">
        <label>
          <input
            type="checkbox"
            checked={isAutoplay}
            onChange={(e) => setIsAutoplay(e.target.checked)}
          />
          Enable Autoplay
        </label>
        {isAutoplay && (
          <div>
            <label>
              Autoplay for (weeks): 
              <input
                type="number"
                value={autoplayWeeks}
                onChange={(e) => setAutoplayWeeks(Number(e.target.value))}
                min={0}
              />
            </label>
          </div>
        )}
      </div>

      {/* Play Button */}
      <button className="play-button" onClick={handlePlay}>
        PLAY
      </button>
    </div>
  );
};

export default Board;
