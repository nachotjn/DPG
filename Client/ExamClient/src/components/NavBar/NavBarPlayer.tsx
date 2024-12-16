import { Link } from "react-router-dom";
import styles from "./navBar.module.css";
import { useNavigate } from "react-router-dom";
import { gameAtom } from "../../store/atoms";
import { useAtom } from "jotai";


// dumb component
export const NavBarPlayer = () => {
  const [game, setGame] = useAtom(gameAtom);
  
  const navigate = useNavigate();
  const handleLogout = () => {
    // Clear the token from localStorage
    localStorage.removeItem("token");
  
   navigate("/login");
  };

 
  return (
    <nav className={styles["navbar"]}>
      {/* Contenedor izquierdo */}
      <div className={styles["navbar-left"]}>
        <div className={styles["navbar-logo"]}>
          <Link to="/player-home">
            <img src="./src/assets/images/logo.png" alt="Club Logo" />
          </Link>
        </div>
        <div className={styles["navbar-divider-logo-week"]}></div>
        <div className={styles["navbar-week"]}>Week {game?.weeknumber}</div>
      </div>

      {/* Navbar Buttons*/}
      <div className={styles["navbar-center"]}>
        <div className={styles["navbar-buttons"]}>
          <Link to="/player-game" className={styles["navbar-game"]}>
            Game
          </Link>
          <Link to="/player-profile" className={styles["navbar-members"]}>
            Profile
          </Link>
          <Link to="/player-history" className={styles["navbar-history"]}>
            History
          </Link>
          <Link to="/player-history" className={styles["navbar-history"]}>
            Balance
          </Link>
          <button
            onClick={handleLogout}
            className={styles["navbar-logout"]}
          >
            Log Out
          </button>
        </div>
      </div>
    </nav>
  );
};
