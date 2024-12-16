import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './playerHistoryView.module.css';
import { NavBarPlayer } from '../../../components/NavBar/NavBarPlayer';

const PlayerHistoryView = () => {
  

  return (
    <div className="admin-home">
      {/* Navbar */}
                <NavBarPlayer/>
    </div>
    );
};

export default PlayerHistoryView;