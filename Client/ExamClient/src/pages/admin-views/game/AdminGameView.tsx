import { useState, useEffect } from "react";
import "./adminGameView.module.css";
import Board from "../../../components/Board/Board";
import { NavBar } from "../../../components/NavBar/NavBar";

const AdminGameView = () => {

  return (
    <>
      {/* Navbar */}
      <NavBar />

      {/* Contenido principal */}
      <Board />
     
    </>
  );
};

export default AdminGameView;
