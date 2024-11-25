import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LogInView from './pages/LoginView';
import PlayersGameView from './pages/PlayersGameView';
import PlayersHomeView from './pages/PlayersHomeView'

const App = () => {
  return (
    <Router>
      <Routes>
        <Route index element={<PlayersHomeView />} />
        <Route path="/login" element={<LogInView />} />
        <Route path="/game" element={<PlayersGameView />} />
        <Route path="/home" element={<PlayersHomeView />} />
      </Routes>
    </Router>
  );
};

export default App;
