
import LoginView from "./views/LoginView.tsx";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import AdminView from "./views/AdminView.tsx";

const App = () => {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<LoginView />} />
          <Route path="/login" element={<LoginView />} />
          <Route path="/admin" element={<AdminView />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
};

export default App;
