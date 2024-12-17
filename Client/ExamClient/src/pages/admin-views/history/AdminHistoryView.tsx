import { useState, useEffect } from 'react';
import { NavBar } from "../../../components/NavBar/NavBar";
import GameDetails from './GameDetails';
import './adminHistoryView.module.css';
import { fetchAllGames } from '../../../services/api';

const AdminHistoryView = () => {
  const [games, setGames] = useState<any[]>([]); 
  const [selectedGameId, setSelectedGameId] = useState<string | null>(null); // Track selected game
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadGames = async () => {
      try {
        const data = await fetchAllGames();
        console.log('Fetched games data:', data);

        if (Array.isArray(data)) {
          const sortedGames = data.sort((a, b) => {
            if (a.year === b.year) {
              return a.weeknumber - b.weeknumber;
            }
            return a.year - b.year; 
          });

          setGames(sortedGames);
        } else {
          console.error("Unexpected data format:", data);
          setError("Unexpected data format from server.");
        }
      } catch (error) {
        console.error("Error fetching games:", error);
        setError("Failed to fetch Games. Please try again.");
      }
    };

    loadGames();
  }, []);

  const renderWinningNumbers = (winningNumbers: any) => {
    if (winningNumbers?.$values && Array.isArray(winningNumbers.$values) && winningNumbers.$values.length > 0) {
      return winningNumbers.$values.join(', ');
    } else {
      return 'No winning numbers';
    }
  };

  const renderCompletionStatus = (isComplete: boolean) => {
    return isComplete ? 'Complete' : 'Incomplete';
  };

  return (
    <div>
      {/* Navbar */}
      <NavBar />

      <div className="main-content">
        <h1 className="title">Games History</h1>

        {error && <p className="error-message">{error}</p>}

        <div className="content-container">
          {/* Table Section */}
          <div className="table-container">
            {games.length === 0 ? (
              <p>Loading games...</p>
            ) : (
              <table className="history-table">
                <thead>
                  <tr>
                    <th>Week</th>
                    <th>Year</th>
                    <th>Numbers</th>
                    <th>Prize</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  {games.map((game) => (
                    <tr 
                      key={game.id}
                      onClick={() => setSelectedGameId(game.gameid)} // Set selected game ID
                      className={selectedGameId === game.gameid ? 'selected-row' : ''}
                    >
                      <td>{game.weeknumber}</td>
                      <td>{game.year}</td>
                      <td>{renderWinningNumbers(game.winningnumbers)}</td>
                      <td>{game.prizesum} Kr.</td>
                      <td>{renderCompletionStatus(game.iscomplete)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>

          {/* Game Details Section */}
          <div className="details-container">
            {selectedGameId ? (
              <GameDetails gameId={selectedGameId} />
            ) : (
              <p>Select a game to view details.</p>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminHistoryView;
