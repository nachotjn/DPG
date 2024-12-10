import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Table, Button } from "react-bootstrap";

interface Player {
  id: number;
  fullName: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
}

const PlayerList: React.FC = () => {
  const [players, setPlayers] = useState<Player[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get("/api/players")
      .then(response => {
        setPlayers(response.data as Player[]); // Especificamos el tipo de los datos
      })
      .catch(error => console.error("Error fetching players:", error));
  }, []);

  const handleEdit = (id: number) => {
    navigate(`/players/edit/${id}`);
  };

  const handleDelete = (id: number) => {
    axios
      .delete(`/api/players/${id}`)
      .then(() => {
        setPlayers(prevPlayers => prevPlayers.filter(player => player.id !== id));
      })
      .catch(error => console.error("Error deleting player:", error));
  };

  return (
    <div>
      <h1>Players</h1>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>#</th>
            <th>Full Name</th>
            <th>Email</th>
            <th>Phone Number</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {players.map(player => (
            <tr key={player.id}>
              <td>{player.id}</td>
              <td>{player.fullName}</td>
              <td>{player.email}</td>
              <td>{player.phoneNumber}</td>
              <td>{player.isActive ? "Active" : "Inactive"}</td>
              <td>
                <Button variant="warning" onClick={() => handleEdit(player.id)}>
                  Edit
                </Button>{" "}
                <Button variant="danger" onClick={() => handleDelete(player.id)}>
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default PlayerList;
