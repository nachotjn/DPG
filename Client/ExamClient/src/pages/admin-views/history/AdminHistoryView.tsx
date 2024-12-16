import { useState, useEffect } from 'react';
import { NavBar } from "../../../components/NavBar/NavBar";
import './adminHistoryView.module.css';
import { fetchAllGames } from '../../../services/api';

const AdminHistoryView = () => {
  const [games, setGames] = useState<any[]>([]); 
  const [error, setError] = useState<string | null>(null);


  useEffect(() => {
      const loadGames = async () => {
        try {
          const data = await fetchAllGames();
          console.log('Fetched games data:', data);
          if (Array.isArray(data)) { 
            setGames(data);
          } else {
            console.error("Unexpected data format:", data);
            setError("Unexpected data format from server.");
          }
        } catch (error) {
          setError("Failed to fetch Games. Please try again.");
        }
      };
    
      loadGames();
    }, []);

    const renderWinningNumbers = (winningNumbers: any) => {
      if (winningNumbers && winningNumbers.$values && Array.isArray(winningNumbers.$values) && winningNumbers.$values.length > 0) {
        return winningNumbers.$values.join(', ');  // Safely access $values if available and non-empty
      } else {
        return 'No winning numbers';  // Fallback message if $values is null, undefined, or empty
      }
    };

    const renderCompletionStatus = (isComplete: boolean) => {
      return isComplete ? 'Complete' : 'Incomplete';
    };

  return (
    <div >
      {/* Navbar */}
      <NavBar/>


      <div className="main-content">
        <h1 className="title">Members</h1>
        {error && <p className="error-message">{error}</p>}
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
              {games.map((game, index) => (
                <tr key={game.id || index}> 
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


    </div>
    );
};

export default AdminHistoryView;