import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './AdminHomeView.css';

const AdminHomeView = () => {
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
          <div className="navbar-divider-logo-week"></div> {/* Línea separadora entre logo y semana */}
          <div className="navbar-week">{currentWeek}</div> {/* Semana dinámica */}
        </div>

        {/* Contenedor central */}
        <div className="navbar-center">
          <div className="navbar-buttons">
            <Link to="/admin-home" className="navbar-button">Home</Link>
            <Link to="/admin-game" className="navbar-button">Game</Link>
            <Link to="/admin-members" className="navbar-button">Members</Link>
            <Link to="/admin-history" className="navbar-button">History</Link>
            <Link to="/login" className="navbar-button">Log Out</Link>
          </div>
        </div>
      </nav>

      {/* Línea roja separadora debajo de navbar */}
      <hr className="navbar-divider" />

      {/* Body */}
      <div className="content">
        <div className="cross-lines">
          <div className="cross-lines-vertical"></div>
          <div className="cross-lines-horizontal"></div>
        </div>

        {/* Botones */}
        <div className="buttons-container">
          <Link to="/admin-game" className="button-item">
            <div className="button-background">
              <span className="button-text">Play 'Dead Pigeons'</span>
            </div>
          </Link>
          <Link to="/admin-members" className="button-item">
            <div className="button-background">
              <span className="button-text">Members Info</span>
            </div>
          </Link>
          <Link to="/admin-history" className="button-item">
            <div className="button-background">
              <span className="button-text">Games History</span>
            </div>
          </Link>
          <Link to="/admin-winners" className="button-item">
            <div className="button-background">
              <span className="button-text">Choose a Winner</span>
            </div>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default AdminHomeView;
