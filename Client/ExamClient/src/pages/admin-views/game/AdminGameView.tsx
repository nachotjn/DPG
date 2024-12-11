import { useState, useEffect } from "react";
import "./adminGameView.module.css";
import Board from "../../../components/Board/Board";
import { NavBar } from "../../../components/NavBar/NavBar";

const AdminGameView = () => {
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
      <NavBar weekNumber={currentWeek} />

      {/* Contenido principal */}
      <Board />
     
    </>
  );
};

export default AdminGameView;
