import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './adminMembersView.module.css';
import { fetchAllPlayers } from '../../../services/api';
import CreatePlayerModal from './CreatePlayerModal'; 

const AdminMembersView = () => {
  const [currentWeek, setCurrentWeek] = useState<string>('');
  const [players, setPlayers] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false); 

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

  return (
    <div className="admin-home">
      {/* Navbar */}
      <nav className="navbar">
        <div className="navbar-left">
          <div className="navbar-logo">
            <Link to="/admin-home">
              <img src="./src/assets/images/logo.png" alt="Club Logo" />
            </Link>
          </div>
          <div className="navbar-divider-logo-week"></div>
          <div className="navbar-week">{currentWeek}</div>
        </div>

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
              </tr>
            </thead>
            <tbody>
              {players.map((player, index) => (
                <tr key={player.id || index}> 
                  {/* This is to replace _ with spaces */}
                  <td>{player.userName.replace(/_/g, ' ')}</td> 
                  <td>{player.email}</td>
                  <td>{player.phoneNumber}</td>
                  <td>
                    <button className="action-button">Edit</button>
                    <button className="action-button">See Transactions</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
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
