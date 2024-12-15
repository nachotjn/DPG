import { useState, useEffect } from "react";
import "./playerGameView.module.css";
import Board from "../../../components/Board/Board";
import { NavBar } from "../../../components/NavBar/NavBar";
import { playerAtom } from "../../../store/atoms";
import { useAtom } from "jotai";
import { fetchAllGames } from "../../../services/api";
import { NavBarPlayer } from "../../../components/NavBar/NavBarPlayer";

const PlayerGameView = () => {
  const [currentWeek, setCurrentWeek] = useState<string>("");
  const [player] = useAtom(playerAtom);


  
  return (
    <>
      {/* Navbar */}
      <NavBarPlayer weekNumber={currentWeek} />

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
