import { useState, useEffect } from 'react';
import { fetchWinnersForGame } from '../../../services/api';

const GameWinnerList = ({ gameId, refresh }: { gameId: string; refresh: number }) => {
  const [winners, setWinners] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadWinners = async () => {
      try {
        const data = await fetchWinnersForGame(gameId);
        setWinners(data);
      } catch (err) {
        setError("Failed to load winners.");
      }
    };

    if (gameId) {
      loadWinners();
    }
  }, [gameId, refresh]); // Re-fetch winners when refresh changes

  return (
    <div>
      {error && <p>{error}</p>}
      <h2>Winners for the week:</h2>
      <ul>
        {winners.map((winner) => (
          <li key={winner.winnerid}>
            <p>Amount: {winner.winningamount}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default GameWinnerList;
