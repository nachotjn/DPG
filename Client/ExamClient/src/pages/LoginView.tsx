import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faLock } from "@fortawesome/free-solid-svg-icons";
import axios from "axios";  // Import axios
import "./LogInView.css";
import { login } from "../services/api";
import { jwtDecode } from "jwt-decode";

const LogInView = () => {
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
      const { token } = response;
  
      // Decode token to extract roles
      const decoded: any = jwtDecode(token);
      const roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]; 
  
      setError(null);
      localStorage.setItem("token", token);
  
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
        <label className="input-label">Username</label>
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
