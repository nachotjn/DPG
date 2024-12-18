import React, { useState, useEffect } from 'react';
import { useAtom } from 'jotai';  // For state management
import './playerHistoryView.module.css';
import { NavBarPlayer } from '../../../components/NavBar/NavBarPlayer';
import { playerAtom } from '../../../store/atoms';
import { fetchGamesForPlayer } from '../../../services/api'; 

const PlayerHistoryView = () => {
  const [player, setPlayer] = useAtom(playerAtom);
  const [games, setGames] = useState<any[]>([]); 
  const [loading, setLoading] = useState<boolean>(true); 

  useEffect(() => {
    if (player) {
      const fetchGames = async () => {
        try {
          setLoading(true); 

          const playerGames = await fetchGamesForPlayer(player.id);

          if (playerGames && playerGames.$values && Array.isArray(playerGames.$values)) {
            setGames(playerGames.$values); 
          } else {
            console.error('Invalid data received:', playerGames);
            setGames([]); 
          }
        } catch (error) {
          console.error('Error fetching games', error);
          setGames([]); 
        } finally {
          setLoading(false); 
        }
      };

      fetchGames();
    }
  }, [player]); 

  const renderCompletionStatus = (isComplete: boolean) => {
    return isComplete ? 'Complete' : 'Incomplete';
  };

  return (
    <div className="admin-home">
      {/* Navbar */}
      <NavBarPlayer />

      {/* Display Player's Game History */}
      <div className="player-history">
        <h2>Games Played by {player?.userName.replace(/_/g, ' ')}</h2>

        {/* Display loading state */}
        {loading ? (
          <p>Loading player game history...</p>
        ) : (
          <>
            {games.length === 0 ? (
              <p>No games found for this player.</p>
            ) : (
              <ul>
                {games.map((game: any) => (
                  <li key={game.gameID}>
                    <div>
                      Week: {game.weeknumber}, Year: {game.year}, Prizesum: {game.prizesum}
                      

                      {game.winningnumbers && game.winningnumbers.$values && Array.isArray(game.winningnumbers.$values) ? (
                        <div>
                          Winning Numbers: {game.winningnumbers.$values.join(', ')}
                        </div>
                      ) : (
                        <div>No winning numbers</div>
                      )}
                      
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </>
        )}
      </div>
    </div>
  );
};

export default PlayerHistoryView;
