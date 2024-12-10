import { Link } from "react-router-dom";
import styles from "./navBar.module.css";

type INavBar = {
  weekNumber: string
}

// dumb component
export const NavBar = (props: INavBar) => {
  return (
    <nav className={styles["navbar"]}>
      {/* Contenedor izquierdo */}
      <div className={styles["navbar-left"]}>
        <div className={styles["navbar-logo"]}>
          <Link to="/admin-home">
            <img src="./src/assets/images/logo.png" alt="Club Logo" />
          </Link>
        </div>
        <div className={styles["navbar-divider-logo-week"]}></div>
        <div className={styles["navbar-week"]}>{props.weekNumber}</div>
      </div>

      {/* Navbar Buttons*/}
      <div className={styles["navbar-center"]}>
        <div className={styles["navbar-buttons"]}>
          <Link to="/admin-game" className={styles["navbar-game"]}>
            Game
          </Link>
          <Link to="/admin-members" className={styles["navbar-members"]}>
            Members
          </Link>
          <Link to="/admin-history" className={styles["navbar-history"]}>
            History
          </Link>
          <Link to="/admin-winners" className={styles["navbar-history"]}>
            Winners
          </Link>
          <Link to="/login" className={styles["navbar-logout"]}>
            Log Out
          </Link>
        </div>
      </div>
    </nav>
  );
};
