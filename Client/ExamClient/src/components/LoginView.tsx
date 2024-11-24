import React, { useState } from "react";
import "./LogInView.css";

const LogInView: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    // logica para manejar el inicio de sesión (conectar con backend aqui)
    console.log("Logging in with", { email, password });
  };

  return (
    <div className="login-container">
      <img
        src="/assets/logo.png" 
        alt="Jerne IF Logo"
        className="login-logo"
      />
      <form className="login-form" onSubmit={handleLogin}>
        <div className="input-group">
          <label htmlFor="email">
            <i className="fas fa-user"></i> {/* Icono */}
          </label>
          <input
            type="email"
            id="email"
            placeholder="admin@jerneif.dk"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="input-group">
          <label htmlFor="password">
            <i className="fas fa-lock"></i> {/* Icono */}
          </label>
          <input
            type="password"
            id="password"
            placeholder="**********"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="login-button">
          LOG IN
        </button>
      </form>
    </div>
  );
};

export default LogInView;
