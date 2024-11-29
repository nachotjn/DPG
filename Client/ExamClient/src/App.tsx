import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LogInView from './pages/LoginView';
import PlayersGameView from './pages/PlayersGameView';
import PlayersHomeView from './pages/PlayersHomeView';
import AdminHomeView from './pages/AdminHomeView';

const App = () => {
  return (
    <Router>
      <Routes>
        <Route index element={<LogInView />} />
        <Route path="/login" element={<LogInView />} />
        <Route path="/game" element={<PlayersGameView />} />
        <Route path="/home" element={<PlayersHomeView />} />
        <Route path="/admin-home" element={<AdminHomeView />} />
        
      </Routes>
    </Router>
  );
};

export default App;
