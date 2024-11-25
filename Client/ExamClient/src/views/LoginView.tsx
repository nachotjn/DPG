import { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faLock } from "@fortawesome/free-solid-svg-icons";
import "./LogInView.css";

const LogInView = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = () => {
    console.log('Username:', username);
    console.log('Password:', password);
  };

  return (
    <div className="login-container">
      <div className="logo-container">
        <img src="./src/assets/images/logo.png" alt="Logo" className="logo" /> 
      </div>
      <h2 className="login-title">Welcome to Jerne IF Esbjerg</h2>
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
      <button className="login-button" onClick={handleLogin}>Log In</button>
    </div>
  );
};

export default LogInView;
