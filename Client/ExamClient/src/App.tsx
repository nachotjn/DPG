
import './App.css';
import LoginView from "./views/LoginView.tsx";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import AdminView from "./views/AdminView.tsx";
import BoardPLayersView from "./views/BoardPLayersView.tsx";


const App = () => {
  return (
    <div className="App">
    
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<LoginView />} />
          <Route path="/login" element={<LoginView />} />
          <Route path="/admin" element={<AdminView />} />
          <Route path="/playersboard" element={<BoardPLayersView />} />

        </Routes>
      </BrowserRouter>
    </div>
  );
};

export default App;
