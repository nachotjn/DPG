import { useState, useEffect } from "react";
import { Table, Button, Modal } from "react-bootstrap";
import { Link } from "react-router-dom";
import { NavBar } from "../../../components/NavBar/NavBar";
import "./adminMembersView.module.css";
import { PlayerModal } from "../../../components/Modal/PlayerModal";

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

const fetchedPlayers: Player[] = [
  {
    id: 1,
    name: "John Doe",
    email: "john.doe@example.com",
    phone: "1234567890",
    password: "password123",
    isActive: true,
    balance: 500,
    transactions: ["T123", "T124"],
    autoplayBoards: [
      { week: 46, numbers: [1, 2, 3, 6, 9, 15], isActive: true },
    ],
  },
  {
    id: 2,
    name: "Jane Smith",
    email: "jane.smith@example.com",
    phone: "0987654321",
    password: "password456",
    isActive: false,
    balance: 200,
    transactions: ["T125"],
    autoplayBoards: [{ week: 46, numbers: [5, 7, 12, 14], isActive: false }],
  },
];

const AdminMembersView = () => {
  const [players, setPlayers] = useState<Player[]>([]);
  const [filteredPlayers, setFilteredPlayers] = useState<Player[]>([]);
  const [showDetailsModal, setShowDetailsModal] = useState<boolean>(false);
  const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
  const [showConfirmModal, setShowConfirmModal] = useState<boolean>(false);
  const [deletePlayerId, setDeletePlayerId] = useState<number | null>(null);
  const [filterStatus, setFilterStatus] = useState<string>("all");
  const [showNewPlayerModal, setShowNewPlayerModal] = useState<boolean>(false);
  const [editPlayer, setEditPlayer] = useState<Player>()

  useEffect(() => {
    // Simulación de datos de jugadores (reemplazar con llamada real a la API)

    setPlayers(fetchedPlayers);
    setFilteredPlayers(fetchedPlayers);
  }, [fetchedPlayers]);

  useEffect(() => {
    if (filterStatus === "active") {
      setFilteredPlayers(players.filter((player) => player.isActive));
    } else if (filterStatus === "inactive") {
      setFilteredPlayers(players.filter((player) => !player.isActive));
    } else {
      setFilteredPlayers(players); // Mostrar todos
    }
  }, [filterStatus, players]);

  const handleToggleActive = (id: number) => {
    setPlayers(
      players.map((player) =>
        player.id === id ? { ...player, isActive: !player.isActive } : player
      )
    );
  };

  const handleShowNewPlayerModal = () => {
    setShowNewPlayerModal(!showNewPlayerModal);
  };

  const handleDeletePlayer = () => {
    if (deletePlayerId !== null) {
      setPlayers(players.filter((player) => player.id !== deletePlayerId));
      setFilteredPlayers(
        filteredPlayers.filter((player) => player.id !== deletePlayerId)
      );
      setShowConfirmModal(false); // Cerrar modal después de eliminar
    }
  };

  const handleShowDetails = (player: Player) => {
    setSelectedPlayer(player);
    setShowDetailsModal(true);
  };

  const handleCloseDetailsModal = () => setShowDetailsModal(false);

  const handleToggleBoardActive = (week: number) => {
    if (selectedPlayer) {
      const updatedBoards = selectedPlayer.autoplayBoards.map((board) =>
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

  const handleEditPlayer = (id: number) => {
    console.log("Selected player:", id);
    const selectedPlayer = fetchedPlayers.filter((player) => player.id === id)
    setEditPlayer(selectedPlayer[0])
    setShowNewPlayerModal(true)
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
    const today = new Date();
    const weekNumber = getWeekOfYear(today);
    setCurrentWeek(`WEEK ${weekNumber}`);
  }, []);

  return (
    <div>
      <NavBar weekNumber={currentWeek} />
      <h2>Manage Players</h2>
      <Button variant="primary" onClick={handleShowNewPlayerModal}>
        Add Player
      </Button>
      <Button
        variant="outline-secondary"
        onClick={() => setFilterStatus("all")}
      >
        Show All Players
      </Button>
      <Button
        variant="outline-success"
        onClick={() => setFilterStatus("active")}
      >
        Show Active Players
      </Button>
      <Button
        variant="outline-danger"
        onClick={() => setFilterStatus("inactive")}
      >
        Show Inactive Players
      </Button>

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
          {filteredPlayers.map((player) => (
            <tr key={player.id}>
              <td>
                <Button
                  variant="link"
                  onClick={() => handleShowDetails(player)}
                >
                  {player.name}
                </Button>
              </td>
              <td>{player.email}</td>
              <td>
                {player.autoplayBoards.map((board) => board.week).join(", ")}
              </td>
              <td>{player.balance} DKK</td>
              <td>
                <Link to={`/transactions/${player.id}`}>View Transactions</Link>
              </td>
              <td>
                <span style={{ color: player.isActive ? "green" : "red" }}>
                  {player.isActive ? "Active" : "Inactive"}
                </span>
                <Button
                  variant={player.isActive ? "danger" : "success"}
                  onClick={() => handleToggleActive(player.id)}
                >
                  {player.isActive ? "Deactivate" : "Activate"}
                </Button>
              </td>
              <td>
                <Button variant="warning" onClick={() => handleEditPlayer(player.id)}>
                  Edit
                </Button>
                <Button
                  variant="danger"
                  onClick={() => {
                    setDeletePlayerId(player.id);
                    setShowConfirmModal(true);
                  }}
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <PlayerModal
        showModal={showNewPlayerModal}
        setModal={setShowNewPlayerModal}
        editPlayer={editPlayer}
      />

      {/* Modal de confirmación para eliminar jugador */}
      <Modal show={showConfirmModal} onHide={() => setShowConfirmModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Confirm Deletion</Modal.Title>
        </Modal.Header>
        <Modal.Body>Are you sure you want to delete this player?</Modal.Body>
        <Modal.Footer>
          <Button
            variant="secondary"
            onClick={() => setShowConfirmModal(false)}
          >
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
            <p>
              <strong>Email:</strong> {selectedPlayer?.email}
            </p>
            <p>
              <strong>Phone:</strong> {selectedPlayer?.phone}
            </p>
            <p>
              <strong>Status:</strong>{" "}
              <span
                style={{ color: selectedPlayer?.isActive ? "green" : "red" }}
              >
                {selectedPlayer?.isActive ? "Active" : "Inactive"}
              </span>
            </p>
            <p>
              <strong>Balance:</strong> {selectedPlayer?.balance} DKK
            </p>
            <p>
              <strong>Transactions:</strong>{" "}
              {selectedPlayer?.transactions.join(", ")}
            </p>

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
                    <td>{board.numbers.join(", ")}</td>
                    <td style={{ color: board.isActive ? "green" : "red" }}>
                      {board.isActive ? "Active" : "Inactive"}
                    </td>
                    <td>
                      <Button
                        variant={board.isActive ? "danger" : "success"}
                        onClick={() => handleToggleBoardActive(board.week)}
                      >
                        {board.isActive ? "Deactivate" : "Activate"}
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
            <Button
              variant="primary"
              onClick={() => handleAddAutoplayBoard(48, [1, 2, 3])}
            >
              Add Autoplay Board
            </Button>
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
};

export default AdminMembersView;
