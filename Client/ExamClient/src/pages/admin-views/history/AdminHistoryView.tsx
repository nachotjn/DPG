import { useState, useEffect } from 'react'; 
import { useAtom } from 'jotai';
import { selectedWeekAtom } from '../../../store/atoms';
import { NavBar } from '../../../components/NavBar/NavBar';
import InfoTable from '../../../components/InfoTable/InfoTable'; 

// Definición de tipo para los datos de la semana
interface WeekData {
  weekNumber: number;
  players: number;
  boards: number;
  prize: number;
}

const AdminHistoryView = () => {
  // Declaración de estados
  const [selectedWeek, setSelectedWeek] = useAtom(selectedWeekAtom);
  const [weeksData, setWeeksData] = useState<WeekData[]>([]);
  const [weekDetails, setWeekDetails] = useState<any>(null);
  const [loading, setLoading] = useState(true);  // Para manejar el estado de carga

  // Llamada a la API para obtener los datos
  useEffect(() => {
    const fetchWeeksData = async () => {
      try {
        const response = await fetch('/api/weeks');
        if (!response.ok) throw new Error('Failed to fetch');
        const data = await response.json();
        setWeeksData(data);
        setLoading(false);  // Cuando los datos se reciben, cambiamos el estado de carga
      } catch (error) {
        console.error('Error fetching data:', error);
        setLoading(false);  // Cambiamos el estado de carga incluso si hay un error
      }
    };
    fetchWeeksData();
  }, []);

  // Maneja la selección de una semana
  const handleWeekSelection = (week: WeekData) => {
    setSelectedWeek(week);
    setWeekDetails({
      winners: [
        { name: 'Jugador 1', prize: 200 },
        { name: 'Jugador 2', prize: 300 },
      ],
      winningNumbers: [5, 7, 12],
    });
  };

  return (
    <div>
      <NavBar weekNumber={selectedWeek?.weekNumber?.toString() ?? ''} />
      <h2>GAMES HISTORY</h2>

      <div className="d-flex">
        {/* Tabla principal con la historia de los juegos */}
        <div className="col-md-6">
          {loading ? (
            <p>Loading data...</p>  // Muestra mensaje mientras se cargan los datos
          ) : (
            <InfoTable 
              headers={['WEEK', 'PLAYERS', 'BOARDS', 'PRIZE']}
              data={weeksData.map(week => ({
                weekNumber: week.weekNumber,
                players: week.players,
                boards: week.boards,
                prize: `${week.prize} DKK`
              }))}
            />
          )}
        </div>

        {/* Tabla con los detalles de la semana seleccionada */}
        {weekDetails && (
          <div className="col-md-6">
            <h3>WEEK {selectedWeek?.weekNumber} DETAILS</h3>
            <InfoTable 
              headers={['Winning Numbers']}
              data={[{ 'Winning Numbers': weekDetails.winningNumbers.join(', ') }]} 
            />
            <InfoTable 
              headers={['Player', 'Prize']}
              data={weekDetails.winners.map((winner: any) => ({
                Player: winner.name,
                Prize: `${winner.prize} DKK`
              }))}
            />
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminHistoryView;
