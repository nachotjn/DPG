import React, { useState, useEffect } from 'react';
import { useAtom } from 'jotai'; // Import the useAtom hook from jotai
import { gameAtom } from '../../../store/atoms'; // Import gameAtom
import { NavBar } from '../../../components/NavBar/NavBar';
import { determineWinnersForGame, fetchAllGames, updateGame } from '../../../services/api'; 
import './adminWinnersView.module.css';
import GameWinnerList from './GameWinnerList';

const AdminWinnersView = () => { 
  const [games, setGames] = useState<any[]>([]);  
  const [game, setGame] = useAtom(gameAtom); 
  const [winningNumbers, setWinningNumbers] = useState<number[]>([]);  
  const [error, setError] = useState<string | null>(null);
  const [refreshWinners, setRefreshWinners] = useState<number>(0); 

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

  useEffect(() => {
    setRefreshWinners((prev) => prev + 1);
  }, [game]);

  
  const handleSelectNumber = (number: number) => {
    if (winningNumbers.length < 3 && !winningNumbers.includes(number)) {
      setWinningNumbers([...winningNumbers, number]);
    } else {
      alert('You can select only 3 unique numbers.');
    }
  };

 
  const handleSubmit = async () => {
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
        alert('Game updated successfully!');
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
      await determineWinnersForGame(game.gameid);
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
                >
                  {number}
                </button>
              );
            })}
          </div>
        </div>

        {/* Display selected numbers */}
        <div>
          <h3>Selected Winning Numbers: {winningNumbers.join(', ')}</h3>
        </div>

        {/* Submit Button */}
        <button onClick={handleSubmit} className="submit-button">
          Update Game
        </button>

        <button onClick={handleDetermineWinners} className="submit-button">
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
