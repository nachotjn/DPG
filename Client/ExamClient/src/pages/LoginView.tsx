import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faLock } from "@fortawesome/free-solid-svg-icons";
import axios from "axios";  // Import axios
import "./LogInView.css";
import { fetchAllGames, fetchAllPlayers, login } from "../services/api";
import { jwtDecode } from "jwt-decode";
import { useAtom } from "jotai";
import { playerAtom, gameAtom } from "../store/atoms";

const getCurrentWeekAndYear = () => {
  const currentDate = new Date();

 
  const startOfYear = new Date(currentDate.getFullYear(), 0, 1);
  const diff = currentDate.getTime() - startOfYear.getTime();
  const oneDay = 1000 * 60 * 60 * 24;
  const days = Math.floor(diff / oneDay);

  
  const weekNumber = Math.ceil((days + 1) / 7);
  const currentYear = currentDate.getFullYear();

  return { weekNumber, currentYear };
};

const LogInView = () => {
  
  const [player, setPlayer] = useAtom(playerAtom);
  const [game, setGame] = useAtom(gameAtom);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleLogin = async () => {
    if (!username || !password) {
      setError("Please provide both email and password.");
      return;
    }
  
    try {
      const response = await login(username, password);
      const { token} = response;
  
      // Decode token to extract roles
      const decoded: any = jwtDecode(token);
      const roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]; 
  
      setError(null);
      localStorage.setItem("token", token);

      const players = await fetchAllPlayers();
      const loggedInPlayer = players.find((p) => p.email === username);  

      const games = await fetchAllGames();
      const { weekNumber, currentYear } = getCurrentWeekAndYear();
      const currentGame = games.find((g) => g.weeknumber === weekNumber && g.year === currentYear);
    

      if (!loggedInPlayer) {
        setError("Player not found.");
        return;
      }

      setPlayer(loggedInPlayer);
      setGame(currentGame);
  
      // Redirect based on roles
      if (roles.includes("Admin")) {
        navigate("/admin-home");
      } else if (roles.includes("Player")) {
        navigate("/player-home");
      } else {
        setError("You dont have permissions to login")
      }
    } catch (err) {
      setError("Invalid username or password");
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLDivElement>) => {
    if (e.key === "Enter") {
      handleLogin();
    }
  };

  return (
    <div className="login-container" onKeyDown={handleKeyDown} tabIndex={0}>
      <div className="logo-container">
        <img src="./src/assets/images/logo.png" alt="Logo" className="logo" />
      </div>
      <h2 className="login-title">Welcome to Jerne IF Esbjerg</h2>
      {error && <p className="error-message">{error}</p>}
      <div className="input-container">
        <label className="input-label">Email</label>
        <div className="input-with-icon">
          <FontAwesomeIcon icon={faUser} className="icon" />
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Enter your username"
            className="input-field"
          />
        </div>
      </div>
      <div className="input-container">
        <label className="input-label">Password</label>
        <div className="input-with-icon">
          <FontAwesomeIcon icon={faLock} className="icon" />
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Enter your password"
            className="input-field"
          />
        </div>
      </div>
      <button className="login-button" onClick={handleLogin}>
        Log In
      </button>
    </div>
  );
};

export default LogInView;
