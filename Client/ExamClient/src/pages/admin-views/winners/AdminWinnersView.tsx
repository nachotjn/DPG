import { useState, useEffect } from 'react';
import { Table, Button } from 'react-bootstrap';
import { NavBar } from '../../../components/NavBar/NavBar';
import SelectWinnersBoard from '../../../components/SelectWinnersBoard/SelectWinnersBoard';
import './adminWinnersView.module.css';

interface Winner {
  id: number;
  name: string;
  boards: number[][];
  prize?: number; // Premio calculado
  isPaid: boolean; // Estado de pago
}

const AdminWinnersView = () => {
  const [currentWinners, setCurrentWinners] = useState<Winner[]>([]);
  const [selectedNumbers, setSelectedNumbers] = useState<number[]>([]);
  const [currentWeek, setCurrentWeek] = useState<string>('');

  // Calcular la semana actual y establecerla
  useEffect(() => {
    const today = new Date();
    const weekNumber = getWeekNumber(today);
    setCurrentWeek(`WEEK ${weekNumber}`);
  }, []);

  const getWeekNumber = (date: Date) => {
    const start = new Date(date.getFullYear(), 0, 1);
    const diff = date.getTime() - start.getTime();
    const oneDay = 1000 * 60 * 60 * 24;
    const days = Math.floor(diff / oneDay);
    return Math.ceil((days + 1) / 7);
  };

  // Confirmar la selección de números ganadores y obtener ganadores de la "base de datos"
  const handleConfirmSelection = async (numbers: number[]) => {
    setSelectedNumbers(numbers);

    try {
      const response = await fetch('/api/get-winners', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ week: currentWeek, numbers })
      });
      const data: Winner[] = await response.json();

      if (data && data.length > 0) {
        setCurrentWinners(data);
      } else {
        setCurrentWinners([]);
      }
    } catch (error) {
      console.error('Error fetching winners:', error);
    }
  };

  // Marcar a un ganador como "pagado" y actualizar su saldo
  const handleMarkAsPaid = async (winnerId: number, prize: number | undefined) => {
    if (!prize) return;

    try {
      await fetch(`/api/update-player-balance`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ winnerId, amount: prize })
      });

      setCurrentWinners((prevWinners) =>
        prevWinners.map((winner) =>
          winner.id === winnerId ? { ...winner, isPaid: true } : winner
        )
      );
    } catch (error) {
      console.error('Error updating player balance:', error);
    }
  };

  // Cancelar la confirmación de pago y restar el monto del saldo del jugador
  const handleCancelPayment = async (winnerId: number, prize: number | undefined) => {
    if (!prize) return;

    try {
      await fetch(`/api/update-player-balance`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ winnerId, amount: -prize })
      });

      setCurrentWinners((prevWinners) =>
        prevWinners.map((winner) =>
          winner.id === winnerId ? { ...winner, isPaid: false } : winner
        )
      );
    } catch (error) {
      console.error('Error canceling payment:', error);
    }
  };

  // Restablecer la selección de números
  const handleResetSelection = () => {
    setSelectedNumbers([]);
    setCurrentWinners([]);
  };

  return (
    <div>
      <NavBar weekNumber={currentWeek} />
      <div className="admin-view d-flex">
        <div className="left-panel w-50 p-3">
          <h3>Select Winning Numbers</h3>
          <SelectWinnersBoard onConfirmSelection={handleConfirmSelection} />
          <p>
            Selected Numbers:{" "}
            {selectedNumbers.length > 0 ? selectedNumbers.join(", ") : "None"}
          </p>
          <Button 
            variant="danger" 
            onClick={handleResetSelection} 
            className="mt-3"
          >
            Reset Selection
          </Button>
        </div>

        <div className="right-panel w-50 p-3">
          <h3>Weekly Winners</h3>
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>Name</th>
                <th>Prize</th>
                <th>Winning Boards</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {currentWinners.length > 0 ? (
                currentWinners.map((winner) => (
                  <tr key={winner.id}>
                    <td>{winner.name}</td>
                    <td>{winner.prize?.toFixed(2)} DKK</td>
                    <td>{winner.boards.map(board => `[${board.join(", ")}]`).join(", ")}</td>
                    <td style={{ color: winner.isPaid ? 'green' : 'red' }}>
                      {winner.isPaid ? 'Paid' : 'Pending'}
                    </td>
                    <td>
                      {!winner.isPaid ? (
                        <Button 
                          variant="success" 
                          onClick={() => handleMarkAsPaid(winner.id, winner.prize)}
                        >
                          Mark as Paid
                        </Button>
                      ) : (
                        <Button 
                          variant="warning" 
                          onClick={() => handleCancelPayment(winner.id, winner.prize)}
                        >
                          Cancel Payment
                        </Button>
                      )}
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={5} className="text-center">
                    No winners for the selected numbers.
                  </td>
                </tr>
              )}
            </tbody>
          </Table>
        </div>
      </div>
    </div>
  );
};

export default AdminWinnersView;
