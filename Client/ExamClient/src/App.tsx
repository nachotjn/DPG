import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LogInView from './pages/LoginView';
import AdminHomeView from './pages/admin-views/home/AdminHomeView';
import AdminGameView from './pages/admin-views/game/AdminGameView';
import AdminMembersView from './pages/admin-views/AdminMembersView';
import AdminHistoryView from './pages/admin-views/history/AdminHistoryView';
import AdminWinnersView from './pages/admin-views/AdminWinnersView';
import PlayerGameView from './pages/player-views/PlayerGameView';
import PlayerHomeView from './pages/player-views/PlayerHomeView';
import PlayerProfileView from './pages/player-views/PlayerProfileView';
import PlayerHistoryView from './pages/player-views/PlayerHistoryView';
import './App.css'
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
      </Routes>
    </Router>
  );
};

export default App;
