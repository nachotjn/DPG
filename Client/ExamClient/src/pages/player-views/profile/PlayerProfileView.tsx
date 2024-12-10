import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './playerProfileView.module.css';

const PlayerProfileView = () => {
  const [currentWeek, setCurrentWeek] = useState<string>('');

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

  return (
    <div className="admin-home">
      {/* Navbar */}
      <nav className="navbar">
        {/* Contenedor izquierdo */}
        <div className="navbar-left">
          <div className="navbar-logo">
            <Link to="/admin-home">
              <img src="./src/assets/images/logo.png" alt="Club Logo" />
            </Link>
          </div>
          <div className="navbar-divider-logo-week"></div>
          <div className="navbar-week">{currentWeek}</div>
        </div>

        {/* Navbar Buttons*/}
        <div className="navbar-center">
          <div className="navbar-buttons">
            <Link to="/admin-game" className="navbar-game">Game</Link>
            <Link to="/admin-members" className="navbar-members">Members</Link>
            <Link to="/admin-history" className="navbar-history">History</Link>
            <Link to="/admin-winners" className="navbar-history">Winners</Link>
            <Link to="/login" className="navbar-logout">Log Out</Link>
          </div>
        </div>
      </nav>
    </div>
    );
};

export default PlayerProfileView;