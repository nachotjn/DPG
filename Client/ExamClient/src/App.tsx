import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LogInView from './views/LoginView';
import PlayersGameView from './views/PlayersGameView';
import PlayersHomeView from './views/PlayersHomeView'

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LogInView />} />
        <Route path="/game" element={<PlayersGameView />} />
        <Route path="/home" element={<PlayersHomeView />} />

      </Routes>
    </Router>
  );
};

export default App;
