import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './adminMembersView.module.css';
import { fetchAllPlayers } from '../../../services/api';
import CreatePlayerModal from './CreatePlayerModal';
import PlayerTransactions from './PlayerTransactions';
import { NavBar } from '../../../components/NavBar/NavBar';

const AdminMembersView = () => {
  const [currentWeek, setCurrentWeek] = useState<string>('');
  const [players, setPlayers] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [selectedPlayerId, setSelectedPlayerId] = useState<string | null>(null); 
  const [isTransactionsVisible, setIsTransactionsVisible] = useState<boolean>(false); 

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
    const loadPlayers = async () => {
      try {
        const data = await fetchAllPlayers();
        if (Array.isArray(data)) {
          setPlayers(data);
        } else {
          console.error("Unexpected data format:", data);
          setError("Unexpected data format from server.");
        }
      } catch (error) {
        setError("Failed to fetch players. Please try again.");
      }
    };

    loadPlayers();
  }, []);

  const openModal = () => setIsModalOpen(true);
  const closeModal = () => setIsModalOpen(false);

  const renderPlayerStatus = (isActive: boolean) => {
    return isActive ? 'Active Member' : 'Not Active';
  };

  const handleSeeTransactions = (playerId: string) => {
    setSelectedPlayerId(playerId); 
    setIsTransactionsVisible(true); 
  };

  return (
    <div className="admin-home">
      {/* Navbar */}
            <NavBar weekNumber={currentWeek} />

      {/* Main Content */}
      <div className="main-content">
        <h1 className="title">Members</h1>
        {error && <p className="error-message">{error}</p>}
        {players.length === 0 ? (
          <p>Loading players...</p>
        ) : (
          <table className="members-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Phone</th>
                <th>Balance</th>
                <th>Member Status</th>
                
              </tr>
            </thead>
            <tbody>
              {players.map((player, index) => (
                <tr key={player.id || index}>
                  {/* Replace underscores with spaces */}
                  <td>{player.userName.replace(/_/g, ' ')}</td>
                  <td>{player.email}</td>
                  <td>{player.phoneNumber}</td>
                  <td>{player.balance} Kr.</td>
                  <td>{renderPlayerStatus(player.isactive)}</td>

                  <td>
                    <button className="action-button">Edit</button>
                    <button className="action-button" onClick={() => handleSeeTransactions(player.id)}>
                      See Transactions
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}

        {/* Transactions Section */}
        {isTransactionsVisible && selectedPlayerId && (
          <div className="transactions-section">
            <PlayerTransactions playerId={selectedPlayerId} />
            <button className="action-button" onClick={() => setIsTransactionsVisible(false)}>
              Close Transactions
            </button>
          </div>
        )}

        {/* Button to trigger modal */}
        <button className="action-button" onClick={openModal}>Create New Player</button>

        {/* Modal to create a new player */}
        <CreatePlayerModal isOpen={isModalOpen} onClose={closeModal} />
      </div>
    </div>
  );
};

export default AdminMembersView;
