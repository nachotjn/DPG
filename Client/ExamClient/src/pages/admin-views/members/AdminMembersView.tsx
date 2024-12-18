import { useState, useEffect } from 'react';
import './adminMembersView.module.css';
import { fetchAllPlayers } from '../../../services/api';
import CreatePlayerModal from './CreatePlayerModal';
import PlayerTransactions from './PlayerTransactions';
import { NavBar } from '../../../components/NavBar/NavBar';
import EditPlayerModal from './EditPlayerModal';
import { useState, useEffect } from 'react';
import { Table, Button, Modal } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { NavBar } from '../../../components/NavBar/NavBar';
import './adminMembersView.module.css';

interface AutoplayBoard {
  week: number;
  numbers: number[];
  isActive: boolean;
}

interface Player {
  id: number;
  name: string;
  email: string;
  phone: string;
  password: string;
  isActive: boolean;
  balance: number;
  transactions: string[];
  autoplayBoards: AutoplayBoard[];
}

const AdminMembersView = () => {
  const [players, setPlayers] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [selectedPlayerId, setSelectedPlayerId] = useState<string | null>(null); 
  const [isTransactionsVisible, setIsTransactionsVisible] = useState<boolean>(false); 
  const [isEditModalOpen, setIsEditModalOpen] = useState<boolean>(false);
  const [selectedPlayer, setSelectedPlayer] = useState<any | null>(null); 
  const [searchQuery, setSearchQuery] = useState<string>(''); 
  const [players, setPlayers] = useState<Player[]>([]);
  const [filteredPlayers, setFilteredPlayers] = useState<Player[]>([]);
  const [showDetailsModal, setShowDetailsModal] = useState<boolean>(false);
  const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
  const [showConfirmModal, setShowConfirmModal] = useState<boolean>(false);
  const [deletePlayerId, setDeletePlayerId] = useState<number | null>(null);
  const [filterStatus, setFilterStatus] = useState<string>("all");

  useEffect(() => {
    // Simulación de datos de jugadores (reemplazar con llamada real a la API)
    const fetchedPlayers: Player[] = [
      { 
        id: 1, 
        name: 'John Doe', 
        email: 'john.doe@example.com', 
        phone: '1234567890', 
        password: 'password123',
        isActive: true, 
        balance: 500, 
        transactions: ['T123', 'T124'],
        autoplayBoards: [{ week: 46, numbers: [1, 2, 3, 6, 9, 15], isActive: true }]
      },
      { 
        id: 2, 
        name: 'Jane Smith', 
        email: 'jane.smith@example.com', 
        phone: '0987654321', 
        password: 'password456',
        isActive: false, 
        balance: 200, 
        transactions: ['T125'],
        autoplayBoards: [{ week: 46, numbers: [5, 7, 12, 14], isActive: false }]
      },
    ];
    setPlayers(fetchedPlayers);
    setFilteredPlayers(fetchedPlayers);
  }, []);

  useEffect(() => {
    if (filterStatus === "active") {
      setFilteredPlayers(players.filter(player => player.isActive));
    } else if (filterStatus === "inactive") {
      setFilteredPlayers(players.filter(player => !player.isActive));
    } else {
      setFilteredPlayers(players); // Mostrar todos
    }
  }, [filterStatus, players]);

  const handleToggleActive = (id: number) => {
    setPlayers(players.map(player =>
      player.id === id ? { ...player, isActive: !player.isActive } : player
    ));
  };

  const handleDeletePlayer = () => {
    if (deletePlayerId !== null) {
      setPlayers(players.filter(player => player.id !== deletePlayerId));
      setFilteredPlayers(filteredPlayers.filter(player => player.id !== deletePlayerId));
      setShowConfirmModal(false);  // Cerrar modal después de eliminar
    }
  };

  const handleShowDetails = (player: Player) => {
    setSelectedPlayer(player);
    setShowDetailsModal(true);
  };

  const handleCloseDetailsModal = () => setShowDetailsModal(false);

  const handleToggleBoardActive = (week: number) => {
    if (selectedPlayer) {
      const updatedBoards = selectedPlayer.autoplayBoards.map(board =>
        board.week === week ? { ...board, isActive: !board.isActive } : board
      );
      setSelectedPlayer({ ...selectedPlayer, autoplayBoards: updatedBoards });
    }
  };

  const handleAddAutoplayBoard = (week: number, numbers: number[]) => {
    if (selectedPlayer) {
      const newBoard: AutoplayBoard = { week, numbers, isActive: true };
      const updatedBoards = [...selectedPlayer.autoplayBoards, newBoard];
      setSelectedPlayer({ ...selectedPlayer, autoplayBoards: updatedBoards });
    }
  };

  const [currentWeek, setCurrentWeek] = useState("");

  const getWeekOfYear = (date: Date) => {
    const start = new Date(date.getFullYear(), 0, 1);
    const diff = date.getTime() - start.getTime();
    const oneDay = 1000 * 60 * 60 * 24;
    const days = Math.floor(diff / oneDay);
    return Math.ceil((days + 1) / 7);
  };

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

  const openEditModal = (player: any) => {
    setSelectedPlayer(player);
    setIsEditModalOpen(true);
  };

  const refreshPlayers = async () => {
    const data = await fetchAllPlayers();
    setPlayers(data);
  };

  const filteredPlayers = players.filter(player =>
    player.userName.toLowerCase().includes(searchQuery.toLowerCase()) ||
    player.email.toLowerCase().includes(searchQuery.toLowerCase())
  );

  
  return (
    <div className="admin-home">
      {/* Navbar */}
      <NavBar/>

      {/* Main Content */}
      <div className="main-content">
        <h1 className="title">Members</h1>
        {error && <p className="error-message">{error}</p>}

        {/* Search Bar */}
        <input 
          type="text" 
          placeholder="Search for players..." 
          className="search-bar" 
          value={searchQuery} 
          onChange={(e) => setSearchQuery(e.target.value)} 
        />

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
              {filteredPlayers.map((player, index) => (
                <tr key={player.id || index}>
                  {/* Replace underscores with spaces */}
                  <td>{player.userName.replace(/_/g, ' ')}</td>
                  <td>{player.email}</td>
                  <td>{player.phoneNumber}</td>
                  <td>{player.balance} Kr.</td>
                  <td>{renderPlayerStatus(player.isactive)}</td>

                  <td>
                    <button className="action-button" onClick={() => openEditModal(player)}>Edit</button>
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
        <CreatePlayerModal 
          isOpen={isModalOpen} 
          onClose={closeModal} 
          refreshPlayers={refreshPlayers} 
        />

        {/* Edit Player Modal */}
        {isEditModalOpen && selectedPlayer && (
          <EditPlayerModal 
            isOpen={isEditModalOpen} 
            onClose={() => setIsEditModalOpen(false)} 
            player={selectedPlayer} 
            refreshPlayers={refreshPlayers} 
          />
        )}
      </div>
    <div>
      <NavBar weekNumber={currentWeek} />
      <h2>Manage Players</h2>
      <Button variant="primary" onClick={() => alert('Add player functionality')}>Add Player</Button>
      <Button variant="outline-secondary" onClick={() => setFilterStatus("all")}>Show All Players</Button>
      <Button variant="outline-success" onClick={() => setFilterStatus("active")}>Show Active Players</Button>
      <Button variant="outline-danger" onClick={() => setFilterStatus("inactive")}>Show Inactive Players</Button>

      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th>MEMBER</th>
            <th>EMAIL</th>
            <th>WEEKS PLAYED</th>
            <th>BALANCE</th>
            <th>TRANSACTIONS</th>
            <th>STATUS</th>
            <th>ACTIONS</th>
          </tr>
        </thead>
        <tbody>
          {filteredPlayers.map(player => (
            <tr key={player.id}>
              <td><Button variant="link" onClick={() => handleShowDetails(player)}>{player.name}</Button></td>
              <td>{player.email}</td>
              <td>{player.autoplayBoards.map(board => board.week).join(', ')}</td>
              <td>{player.balance} DKK</td>
              <td>
                <Link to={`/transactions/${player.id}`}>View Transactions</Link>
              </td>
              <td>
                <span style={{ color: player.isActive ? 'green' : 'red' }}>
                  {player.isActive ? 'Active' : 'Inactive'}
                </span>
                <Button variant={player.isActive ? 'danger' : 'success'} onClick={() => handleToggleActive(player.id)}>
                  {player.isActive ? 'Deactivate' : 'Activate'}
                </Button>
              </td>
              <td>
                <Button variant="warning" onClick={() => alert('Edit player functionality')}>Edit</Button>
                <Button variant="danger" onClick={() => { setDeletePlayerId(player.id); setShowConfirmModal(true); }}>Delete</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {/* Modal de confirmación para eliminar jugador */}
      <Modal show={showConfirmModal} onHide={() => setShowConfirmModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Confirm Deletion</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          Are you sure you want to delete this player?
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowConfirmModal(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleDeletePlayer}>
            Delete
          </Button>
        </Modal.Footer>
      </Modal>

      {/* Modal para detalles del jugador */}
      <Modal show={showDetailsModal} onHide={handleCloseDetailsModal}>
        <Modal.Header closeButton>
          <Modal.Title>{selectedPlayer?.name}'s Details</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <div>
            <p><strong>Email:</strong> {selectedPlayer?.email}</p>
            <p><strong>Phone:</strong> {selectedPlayer?.phone}</p>
            <p><strong>Status:</strong> <span style={{ color: selectedPlayer?.isActive ? 'green' : 'red' }}>{selectedPlayer?.isActive ? 'Active' : 'Inactive'}</span></p>
            <p><strong>Balance:</strong> {selectedPlayer?.balance} DKK</p>
            <p><strong>Transactions:</strong> {selectedPlayer?.transactions.join(', ')}</p>
            
            <h4>Autoplay Boards:</h4>
            <Table striped bordered hover>
              <thead>
                <tr>
                  <th>Week</th>
                  <th>Numbers</th>
                  <th>Status</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {selectedPlayer?.autoplayBoards.map((board, index) => (
                  <tr key={index}>
                    <td>{board.week}</td>
                    <td>{board.numbers.join(', ')}</td>
                    <td style={{ color: board.isActive ? 'green' : 'red' }}>
                      {board.isActive ? 'Active' : 'Inactive'}
                    </td>
                    <td>
                      <Button variant={board.isActive ? 'danger' : 'success'} onClick={() => handleToggleBoardActive(board.week)}>
                        {board.isActive ? 'Deactivate' : 'Activate'}
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
            <Button variant="primary" onClick={() => handleAddAutoplayBoard(48, [1, 2, 3])}>Add Autoplay Board</Button>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseDetailsModal}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
  );
};

export default AdminMembersView;

