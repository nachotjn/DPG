import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LogInView from './pages/LoginView';
import PlayersGameView from './pages/PlayersGameView';
import PlayersHomeView from './pages/PlayersHomeView';
import AdminHomeView from './pages/AdminHomeView';
import AdminHistoryView from './pages/AdminHistoryView';
import AdminWinnersView from './pages/AdminWinnersView';
import AdminMembersView from './pages/AdminMembersView';
import AdminGameView from './pages/AdminGameView';



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
        <Route path="/player-home" element={<PlayersHomeView />} />
        <Route path="/player-game" element={<PlayersGameView />} />

        
      </Routes>
    </Router>
  );
};

export default App;
