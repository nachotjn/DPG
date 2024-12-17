import { useState, useEffect } from 'react';
import { fetchWinnersWithPlayerNames } from '../../../services/api';

const GameWinnerList = ({ gameId, refresh }: { gameId: string; refresh: number }) => {
  const [winners, setWinners] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadData = async () => {
      try {
        const winnersData = await fetchWinnersWithPlayerNames(gameId);
        setWinners(winnersData);
      } catch (err) {
        setError('Failed to load data.');
      }
    };

    if (gameId) {
      loadData();
    }
  }, [gameId, refresh]);

  return (
    <div>
      {error && <p>{error}</p>}
      <h2>Winners for the week:</h2>
      <ul>
        {winners.map((winner) => (
          <li key={winner.winnerid}>
            <p>Player: {winner.playerName.replace(/_/g, ' ')}</p>
            <p>Amount: {winner.winningamount} Kr.</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default GameWinnerList;
