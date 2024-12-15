import React, { useState, useEffect } from 'react';
import { useAtom } from 'jotai';
import { playerAtom } from '../../../store/atoms'; 
import { fetchAllPlayers, updatePlayer } from '../../../services/api'; 
import { changePassword } from '../../../services/api'; 
import { NavBarPlayer } from '../../../components/NavBar/NavBarPlayer';
import './playerProfileView.module.css';

const PlayerProfileView = () => {
  const [player, setPlayer] = useAtom(playerAtom); 
  const [playerData, setPlayerData] = useState({
    userName: '',
    email: '',
    phoneNumber: '',
    balance: 0,
    isactive: false,
  });
  
  const [loading, setLoading] = useState(true); 
  const [passwordData, setPasswordData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  });

  useEffect(() => {
    if (player) {
      setPlayerData({
        userName: player.userName.replace(/_/g, ' '),  
        email: player.email,
        phoneNumber: player.phoneNumber,
        balance: player.balance,
        isactive: player.isactive,
      });
      setLoading(false); 
    }
  }, [player]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setPlayerData((prevData) => ({
      ...prevData,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setPasswordData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleUpdate = async () => {
    if (!player) {
      alert("No player data available.");
      return;
    }

    const updatedPlayerData = {
      playerId: player.id,
      name: playerData.userName.replace(/ /g, '_'),  
      email: playerData.email,
      phone: playerData.phoneNumber,
      isAdmin: false, 
      isActive: playerData.isactive,
      balance: playerData.balance,
      updatedat: new Date().toISOString(),
    };

    try {
      setLoading(true);
      await updatePlayer(player.id, updatedPlayerData);
      
      const players = await fetchAllPlayers();
      const updatedPlayer = players.find((p) => p.id === player.id); 

      if (updatedPlayer) {
        setPlayer(updatedPlayer);
      }

      setLoading(false);
      alert("Player information updated successfully!");
    } catch (error) {
      setLoading(false);
      alert("Failed to update player information.");
    }
  };

  const handleChangePassword = async () => {
    if (passwordData.newPassword !== passwordData.confirmNewPassword) {
      alert("New password and confirmation do not match.");
      return;
    }

    if (!passwordData.currentPassword || !passwordData.newPassword) {
      alert("Please fill out all password fields.");
      return;
    }

    try {
      await changePassword(passwordData.currentPassword, passwordData.newPassword);
      alert("Password changed successfully.");
      setPasswordData({
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: '',
      });
    } catch (error) {
      alert("Failed to change password.");
    }
  };

  return (
    <div className="admin-home">
      {/* Navbar */}
      <NavBarPlayer />

      {/* Player Profile Form */}
      <div className="profile-form">
        <h2>Player Profile</h2>
        {loading ? (
          <p>Loading player profile...</p> 
        ) : (
          <form className="form">
            {/* Existing Player Info Fields */}
            <div className="form-group">
              <label>Name:</label>
              <input
                type="text"
                name="userName"
                value={playerData.userName}
                onChange={handleChange}
              />
            </div>

            <div className="form-group">
              <label>Email:</label>
              <input
                type="email"
                name="email"
                value={playerData.email}
                onChange={handleChange}
              />
            </div>

            <div className="form-group">
              <label>Phone:</label>
              <input
                type="tel"
                name="phoneNumber"
                value={playerData.phoneNumber}
                onChange={handleChange}
              />
            </div>

            <div className="form-group">
              <label>Balance:</label>
              <input
                type="number"
                name="balance"
                value={playerData.balance}
                onChange={handleChange}
                disabled 
              />
            </div>

            <div className="form-group">
              <label>Active Status:</label>
              <input
                type="checkbox"
                name="isactive"
                checked={playerData.isactive}
                onChange={handleChange}
                disabled
              />
            </div>

            <button type="button" onClick={handleUpdate}>Update</button>

            {/* Password Change Section */}
            <h3>Change Password</h3>
            <div className="form-group">
              <label>Current Password:</label>
              <input
                type="password"
                name="currentPassword"
                value={passwordData.currentPassword}
                onChange={handlePasswordChange}
              />
            </div>

            <div className="form-group">
              <label>New Password:</label>
              <input
                type="password"
                name="newPassword"
                value={passwordData.newPassword}
                onChange={handlePasswordChange}
              />
            </div>

            <div className="form-group">
              <label>Confirm New Password:</label>
              <input
                type="password"
                name="confirmNewPassword"
                value={passwordData.confirmNewPassword}
                onChange={handlePasswordChange}
              />
            </div>

            <button type="button" onClick={handleChangePassword}>Change Password</button>
          </form>
        )}
      </div>
    </div>
  );
};

export default PlayerProfileView;
