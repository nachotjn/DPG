import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './playerHistoryView.module.css';
import { NavBarPlayer } from '../../../components/NavBar/NavBarPlayer';

const PlayerHistoryView = () => {
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
                <NavBarPlayer weekNumber={currentWeek} />
    </div>
    );
};

export default PlayerHistoryView;