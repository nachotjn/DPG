import { useState, useEffect } from 'react';
import { Table } from 'react-bootstrap';
import { NavBar } from '../../../components/NavBar/NavBar';
import './adminHistoryView.module.css';

interface WeekData {
  weekNumber: number;
  players: number;
  boards: number;
  prize: number;
}

interface WinnerData {
  name: string;
  prize: number;
}

const AdminHistoryView = () => {
  const [weeksData, setWeeksData] = useState<WeekData[]>([]);
  const [selectedWeek, setSelectedWeek] = useState<WeekData | null>(null);
  const [weekDetails, setWeekDetails] = useState<WinnerData[]>([]);

  useEffect(() => {
    const fetchWeeksData = async () => {
      const data: WeekData[] = [
        { weekNumber: 46, players: 25, boards: 30, prize: 12340 },
        { weekNumber: 47, players: 20, boards: 22, prize: 10250 },
        { weekNumber: 48, players: 28, boards: 35, prize: 15000 },
      ];
      setWeeksData(data);
    };

    fetchWeeksData();
  }, []);

  useEffect(() => {
    if (selectedWeek) {
      const details: WinnerData[] = [
        { name: 'Player 1', prize: 5000 },
        { name: 'Player 2', prize: 3000 },
      ];
      setWeekDetails(details);
    }
  }, [selectedWeek]);

  const handleWeekSelection = (week: WeekData) => {
    setSelectedWeek(week);
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
      <div className="table-container">
        <div className="weeks-table">
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>WEEK</th>
                <th>PLAYERS</th>
                <th>BOARDS</th>
                <th>PRIZE</th>
              </tr>
            </thead>
            <tbody>
              {weeksData.map((week) => (
                <tr
                  key={week.weekNumber}
                  className={selectedWeek?.weekNumber === week.weekNumber ? 'selected-row' : ''}
                  onClick={() => handleWeekSelection(week)}
                >
                  <td>{week.weekNumber}</td>
                  <td>{week.players}</td>
                  <td>{week.boards}</td>
                  <td>{week.prize} DKK</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </div>

        <div className="vertical-separator"></div>

        <div className="details-table">
          {selectedWeek && (
            <>
              <h4>WEEK {selectedWeek.weekNumber} DETAILS</h4>
              <Table striped bordered hover>
                <thead>
                  <tr>
                    <th>Winning Numbers</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td>{[5, 7, 12].join(', ')}</td> {/* Simulated data */}
                  </tr>
                </tbody>
              </Table>

              <Table striped bordered hover>
                <thead>
                  <tr>
                    <th>Player</th>
                    <th>Prize</th>
                  </tr>
                </thead>
                <tbody>
                  {weekDetails.map((winner, index) => (
                    <tr key={index}>
                      <td>{winner.name}</td>
                      <td>{winner.prize} DKK</td>
                    </tr>
                  ))}
                </tbody>
              </Table>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default AdminHistoryView;
