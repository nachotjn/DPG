import { useState, useEffect } from 'react';
import { NavBar } from "../../../components/NavBar/NavBar";
import './adminHistoryView.module.css';
import { fetchAllGames } from '../../../services/api';

const AdminHistoryView = () => {
  const [currentWeek, setCurrentWeek] = useState<string>('');
  const [games, setGames] = useState<any[]>([]); 
  const [error, setError] = useState<string | null>(null);

  const getWeekOfYear = (date: Date) => {
    const start = new Date(date.getFullYear(), 0, 1);
    const diff = date.getTime() - start.getTime();
    const oneDay = 1000 * 60 * 60 * 24;
    const days = Math.floor(diff / oneDay);
    return Math.ceil((days + 1) / 7);
  };

  useEffect(() => {
    const today = new Date();
    const weekNumber = getWeekOfYear(today);
    setCurrentWeek(`WEEK ${weekNumber}`);
  }, []);

  useEffect(() => {
      const loadGames = async () => {
        try {
          const data = await fetchAllGames();
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

    const renderWinningNumbers = (winningNumbers: any[]) => {
      if (winningNumbers && winningNumbers.length > 0) {
        return winningNumbers.join(', '); 
      } else {
        return 'No winning numbers'; 
      }
    };

    const renderCompletionStatus = (isComplete: boolean) => {
      return isComplete ? 'Complete' : 'Incomplete';
    };

  return (
    <div className="admin-home">
      {/* Navbar */}
      <NavBar weekNumber={currentWeek} />


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
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {games.map((game, index) => (
                <tr key={game.id || index}> 
                  <td>{game.weeknumber}</td> 
                  <td>{game.year}</td>
                  <td>{renderWinningNumbers(game.winningnumbers.$values)}</td> 
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