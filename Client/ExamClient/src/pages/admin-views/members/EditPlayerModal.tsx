import React, { useState } from 'react';
import { updatePlayer } from '../../../services/api'; 

const EditPlayerModal = ({ isOpen, onClose, player, refreshPlayers }: any) => {
  const [playerData, setPlayerData] = useState({
    userName: player.userName.replace(/_/g, ' '), 
    email: player.email,
    phoneNumber: player.phoneNumber,
    balance: player.balance,
    isactive: player.isactive,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setPlayerData((prevData: any) => ({
      ...prevData,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleUpdate = async () => {
    setLoading(true);
    try {
      const updatedPlayerData = {
        playerId: player.id,
        name: playerData.userName.replace(/ /g, '_'), 
        email: playerData.email,
        phone: playerData.phoneNumber,
        isAdmin: player.isAdmin,
        isActive: playerData.isactive,
        balance: playerData.balance,
        updatedat: new Date().toISOString(),
      };
      await updatePlayer(player.id, updatedPlayerData);
      refreshPlayers(); 
      onClose(); 
      setLoading(false);
      alert("Player updated successfully!");
    } catch (error) {
      setLoading(false);
      setError("Failed to update player. Please try again.");
    }
  };

  if (!isOpen) return null; 

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>Edit Player</h2>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <form>
          <div>
            <label>Name:</label>
            <input
              type="text"
              name="userName"
              value={playerData.userName}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label>Email:</label>
            <input
              type="email"
              name="email"
              value={playerData.email}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label>Phone:</label>
            <input
              type="text"
              name="phoneNumber"
              value={playerData.phoneNumber}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label>Balance:</label>
            <input
              type="number"
              name="balance"
              value={playerData.balance}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label>Active Status:</label>
            <input
              type="checkbox"
              name="isactive"
              checked={playerData.isactive}
              onChange={handleChange}
            />
          </div>
          <button type="button" onClick={handleUpdate} disabled={loading}>
            {loading ? 'Updating...' : 'Update Player'}
          </button>
          <button type="button" onClick={onClose}>
            Close
          </button>
        </form>
      </div>
    </div>
  );
};

export default EditPlayerModal;
