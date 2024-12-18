import React, { useState, useEffect } from "react";
import { fetchPlayersAndBoardsForGame } from "../../../services/api";

interface GameDetailsProps {
  gameId: string;
}

const GameDetails: React.FC<GameDetailsProps> = ({ gameId }) => {
  const [playersWithBoards, setPlayersWithBoards] = useState<any[]>([]);
  const [winningNumbers, setWinningNumbers] = useState<number[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadGameDetails = async () => {
      try {
        const data = await fetchPlayersAndBoardsForGame(gameId);
        console.log("Fetched players and boards:", data);

        if (data?.length > 0 && data[0]?.boards?.[0]?.game?.winningnumbers?.$values) {
          setWinningNumbers(data[0].boards[0].game.winningnumbers.$values);
        }

        setPlayersWithBoards(data);
      } catch (error) {
        console.error("Error fetching game details:", error);
        setError("Failed to load game details. Please try again.");
      }
    };

    if (gameId) {
      loadGameDetails();
    }
  }, [gameId]); 

  const isWinningBoard = (boardNumbers: number[], winningNumbers: number[]): boolean => {
    return winningNumbers.every((num) => boardNumbers.includes(num));
  };

  const renderBoards = (boards: any[], winningNumbers: number[]) => {
    console.log("Rendering boards for player:", boards); // Log the boards for debugging
  
    if (!boards || boards.length === 0) {
      return <li>No boards found for this player.</li>;
    }
  
    return boards.map((board: any, index: number) => {
      // Handle the case where 'numbers' is a reference (i.e., '$ref')
      let numbers = [];
      if (board.numbers && board.numbers.$values) {
        // If it's already an array
        numbers = board.numbers.$values;
      } else if (board.numbers && board.numbers.$ref) {
        // If it's a reference, attempt to resolve it (you may need another API call)
        console.warn(`Found reference to numbers: ${board.numbers.$ref}`);
        // You might need to fetch or resolve this $ref, depending on your API
      }
  
      if (numbers.length === 0) {
        return (
          <li key={board.boardId} style={{ color: "black" }}>
            Board {index + 1}: No numbers available
          </li>
        );
      }
  
      const isWinner = isWinningBoard(numbers, winningNumbers);
  
      return (
        <li key={board.boardId} style={{ color: isWinner ? "green" : "black" }}>
          Board {index + 1}: Numbers: {numbers.join(", ")} {isWinner ? "(Winning Board)" : ""}
        </li>
      );
    });
  };
  
  

  const totalWinningBoards = playersWithBoards.reduce((count, player) => {
    const winningBoards = player.boards.filter((board: any) =>
      isWinningBoard(
        Array.isArray(board.numbers?.$values) ? board.numbers.$values : [],
        winningNumbers
      )
    );
    return count + winningBoards.length;
  }, 0);

  return (
    <div className="game-details">
      {error && <p className="error-message">{error}</p>}

      <h2>Players and Boards for Game {gameId}</h2>

      {winningNumbers.length > 0 && (
        <p>
          Winning Numbers: <strong>{winningNumbers.join(", ")}</strong>
        </p>
      )}

      {playersWithBoards.length === 0 ? (
        <p>No players found for this game.</p>
      ) : (
        <div>
          <p>
            Total Winning Boards: <strong>{totalWinningBoards}</strong>
          </p>
          <ul>
            {playersWithBoards.map((player) => (
              <li key={player.playerId}>
                {player.name.replace(/_/g, " ")}
                <h4>Boards</h4>
                <ul>{renderBoards(player.boards, winningNumbers)}</ul>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default GameDetails;
