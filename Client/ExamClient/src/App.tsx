import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LogInView from './pages/LoginView';
import AdminHomeView from './pages/admin-views/home/AdminHomeView';
import AdminGameView from './pages/admin-views/game/AdminGameView';
import AdminMembersView from './pages/admin-views/members/AdminMembersView';
import AdminHistoryView from './pages/admin-views/history/AdminHistoryView';
import AdminWinnersView from './pages/admin-views/winners/AdminWinnersView';
import PlayerGameView from './pages/player-views/game/PlayerGameView';
import PlayerHomeView from './pages/player-views/home/PlayerHomeView';
import PlayerProfileView from './pages/player-views/profile/PlayerProfileView';
import PlayerHistoryView from './pages/player-views/history/PlayerHistoryView';
import PlayerList from './components/Player/PlayerList'; 
import PlayerForm from './components/Player/PlayerForm';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const App = () => {
  return (
    <Router>
      <Routes>
        <Route index element={<LogInView />} />
        <Route path="/login" element={<LogInView />} />
        <Route path="/admin-home" element={<AdminHomeView />} />
        <Route path="/admin-game" element={<AdminGameView />} />
        <Route path="/admin-members" element={<AdminMembersView />} />
        <Route path="/admin-history" element={<AdminHistoryView />} />
        <Route path="/admin-winners" element={<AdminWinnersView />} />
        <Route path="/player-home" element={<PlayerHomeView />} />
        <Route path="/player-game" element={<PlayerGameView />} />
        <Route path="/player-profile" element={<PlayerProfileView />} />
        <Route path="/player-history" element={<PlayerHistoryView />} />
        
        {/* Nuevas rutas para gestionar jugadores */}
        <Route path="/players" element={<PlayerList />} /> {/* Listado de jugadores */}
        <Route path="/players/add" element={<PlayerForm />} /> {/* Agregar un nuevo jugador */}
        <Route path="/players/edit/:id" element={<PlayerForm />} /> {/* Editar un jugador */}
      </Routes>
    </Router>
  );
};

export default App;
