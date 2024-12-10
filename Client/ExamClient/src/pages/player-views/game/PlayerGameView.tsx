import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import "./PlayerGameView.css";
import Board from "../../components/Board";
import logo from "../../assets/images/logo.png"; 

const PlayerGameView = () => {
  const [currentWeek, setCurrentWeek] = useState<string>("");

  // Cálculo de la semana actual del año
  const getWeekOfYear = (date: Date) => {
    const start = new Date(date.getFullYear(), 0, 1);
    const diff = date.getTime() - start.getTime();
    const oneDay = 1000 * 60 * 60 * 24;
    const days = Math.floor(diff / oneDay);
    return Math.ceil((days + 1) / 7);
  };

  useEffect(() => {
    const today = new Date();
    const weekNumber = getWeekOfYear(today);
    setCurrentWeek(`WEEK ${weekNumber}`);
  }, []);

  return (
    <>
      {/* Navbar */}
      <div className="admin-home">
        <nav className="navbar">
          {/* Contenedor izquierdo */}
          <div className="navbar-left">
            <div className="navbar-logo">
              <Link to="/admin-home">
                <img src={logo} alt="Club Logo" />
              </Link>
            </div>
            <div className="navbar-divider-logo-week"></div>
            <div className="navbar-week">{currentWeek}</div>
          </div>

          {/* Botones de navegación */}
          <div className="navbar-center">
            <div className="navbar-buttons">
              <Link to="/admin-game" className="navbar-game">Game</Link>
              <Link to="/admin-members" className="navbar-members">Members</Link>
              <Link to="/admin-history" className="navbar-history">History</Link>
              <Link to="/admin-winners" className="navbar-winners">Winners</Link>
              <Link to="/login" className="navbar-logout">Log Out</Link>
            </div>
          </div>
        </nav>
      </div>

      {/* Contenido principal */}
      <section
        style={{
          width: "100vw",
          height: "100vh",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          flexDirection: "column",
        }}
      >
        <div
          style={{
            height: "60vh",
            display: "flex",
            justifyContent: "space-evenly",
            alignItems: "center",
            flexDirection: "column",
          }}
        >
          <Board />

        </div>
      </section>
    </>
  );
};

export default PlayerGameView;
