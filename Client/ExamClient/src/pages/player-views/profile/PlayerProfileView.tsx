import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './playerProfileView.module.css';
import { NavBarPlayer } from '../../../components/NavBar/NavBarPlayer';

const PlayerProfileView = () => {


  return (
    <div className="admin-home">
      {/* Navbar */}
          <NavBarPlayer/>
    </div>
    );
};

export default PlayerProfileView;