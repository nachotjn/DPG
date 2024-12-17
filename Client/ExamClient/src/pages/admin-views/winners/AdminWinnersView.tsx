import React, { useState, useEffect } from 'react';
import { useAtom } from 'jotai'; // Import the useAtom hook from jotai
import { gameAtom } from '../../../store/atoms'; // Import gameAtom
import { NavBar } from '../../../components/NavBar/NavBar';
import { determineWinnersForGame, fetchAllGames, updateGame, fetchWinnersForGame } from '../../../services/api'; 
import './adminWinnersView.module.css';
import GameWinnerList from './GameWinnerList';

const AdminWinnersView = () => { 
  const [games, setGames] = useState<any[]>([]);  
  const [game, setGame] = useAtom(gameAtom); 
  const [winningNumbers, setWinningNumbers] = useState<number[]>([]);  
  const [error, setError] = useState<string | null>(null);
  const [refreshWinners, setRefreshWinners] = useState<number>(0); 

  // Fetch all games on load
  useEffect(() => {
    const loadGames = async () => {
      try {
        const data = await fetchAllGames();
        if (Array.isArray(data)) {
          setGames(data);
        } else {
          console.error('Unexpected data format:', data);
          setError('Unexpected data format from server.');
        }
      } catch (error) {
        setError('Failed to fetch Games. Please try again.');
      }
    };

    loadGames();
  }, []);

  // Re-fetch winners when the selected game changes
  useEffect(() => {
    setRefreshWinners((prev) => prev + 1);
  }, [game]);

  // Check if the game is complete
  const isGameComplete = game?.iscomplete === true;

  const gameHasWinningNumbers = game?.winningnumbers && game?.winningnumbers.length > 0;

  const handleSelectNumber = (number: number) => {
    if (isGameComplete) {
      alert('This game is already complete. You cannot select numbers.');
      return;
    }

    if (winningNumbers.length < 3 && !winningNumbers.includes(number)) {
      setWinningNumbers([...winningNumbers, number]);
    } else {
      alert('You can select only 3 unique numbers.');
    }
  };

  const handleSubmit = async () => {
    if (isGameComplete) {
      alert('This game is already complete. You cannot submit winning numbers.');
      return;
    }

    if (game && winningNumbers.length === 3) {
      const gameToUpdate = {
        gameID: game.gameid,
        winningnumbers: winningNumbers,
        iscomplete: true,
        weeknumber: game.weeknumber,  
        year: game.year,               
        prizesum: game.prizesum,                          
        updatedat: new Date().toISOString(),
      };

      try {
        console.log('Updating game with ID:', game.gameid); 
        await updateGame(game.gameid, gameToUpdate);
        
        // Manually update the game state after the update to reflect the new state
        setGame({ ...game, ...gameToUpdate });  // Update the local game state with the new game state
        
        alert('Winning numbers selected successfully!');
      } catch (error) {
        setError('Error updating game.');
      }
    } else {
      setError('Please select a game and 3 winning numbers.');
    }
  };

  const handleDetermineWinners = async () => {
    if (!game) {
      setError('No current game');
      return;
    }

    try {
      // Fetch winners for the game before determining winners
      const winners = await fetchWinnersForGame(game.gameid);
      
      if (winners && winners.length > 0) {
        // If there are winners already, show an alert
        alert('Winners have already been determined for this game.');
        return; // Don't proceed further if there are already winners
      }

      // If no winners exist, proceed with determining winners
      await determineWinnersForGame(game.gameid);
      
      // After determining winners, fetch the latest game data and update the state
      const updatedGame = await fetchAllGames().then(data => data.find(g => g.gameid === game.gameid));
      
      // Update the game state with the fetched game data
      if (updatedGame) {
        setGame(updatedGame);  // Manually update the local game state after determining winners
      }
      
      alert(`Winners determined for game ${game.gameid}`);
      setRefreshWinners((prev) => prev + 1); 
    } catch (error) {
      setError('Failed to determine winners for this game.');
    }
  };

  return (
    <div className="admin-home">
      {/* Navbar */}
      <NavBar />

      <div className="main-content">
        <h1 className="title">Select Winning Numbers</h1>

        {error && <p className="error-message">{error}</p>}

        {/* Number selection for winning numbers */}
        <div>
          <h2>Selecting for week {game?.weeknumber} of {game?.year}</h2>
          <div className="number-selection">
            {[...Array(18).keys()].map((i) => {
              const number = i + 1;
              return (
                <button
                  key={number}
                  className={`number-button ${winningNumbers.includes(number) ? 'selected' : ''}`}
                  onClick={() => handleSelectNumber(number)}
                  disabled={isGameComplete || winningNumbers.length >= 3}
                >
                  {number}
                </button>
              );
            })}
          </div>
        </div>

        {/* Display selected numbers */}
        <div>
          <h3>Selected Winning Numbers: {gameHasWinningNumbers ? game.winningnumbers.join(', ') : winningNumbers.join(', ')}</h3>
        </div>

        {/* Submit Button */}
        <button 
          onClick={handleSubmit} 
          className="submit-button" 
          disabled={isGameComplete || winningNumbers.length !== 3}
        >
          Select Numbers
        </button>

        <button 
          onClick={handleDetermineWinners} 
          className="submit-button"
        >
          Determine Winners
        </button>

        {/* Winners Section */}
        <div className="winners-section">
          {game?.gameid ? (
            <GameWinnerList gameId={game.gameid} refresh={refreshWinners} />
          ) : (
            <p>Select a game to view winners.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default AdminWinnersView;
