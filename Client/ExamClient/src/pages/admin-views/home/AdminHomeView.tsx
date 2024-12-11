import { useState, useEffect } from "react";
import styles from "./adminHomeView.module.css";
import { Container, Stack } from "react-bootstrap";
import { NavBar } from "../../../components/NavBar/NavBar";
import { ButtonCard } from "../../../components/ButtonCard/ButtonCard";

const AdminHomeView = () => {
  const [currentWeek, setCurrentWeek] = useState("");

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
    <div className={styles["admin-home"]}>
      {/* Navbar */}
      <NavBar weekNumber={currentWeek} />

      <div className={styles["page-body"]}>
        <div className={styles["background-img"]}></div>

        <Container className="d-flex justify-content-center">
          <div className={styles["buttons-container"]}>
            <Stack direction="horizontal" gap={5}>
              <Stack gap={5}>
                <ButtonCard
                  text={"PLAY DEAD PIGEONS"}
                  cssClass={"button-text-1"}
                  to="/admin-game"
                />
                <ButtonCard
                  text={"GAMES HISTORY"}
                  cssClass={"button-text-3"}
                  to="/admin-history"
                />
              </Stack>
              <div className="vr" />
              <Stack gap={5}>
                <ButtonCard
                  text={"MEMBERS INFO"}
                  cssClass={"button-text-2"}
                  to="/admin-members"
                />
                <ButtonCard
                  text={"WINNING NUMBERS"}
                  cssClass={"button-text-4"}
                  to="/admin-winners"
                />
              </Stack>
            </Stack>
          </div>
        </Container>
      </div>
    </div>
  );
};

export default AdminHomeView;
