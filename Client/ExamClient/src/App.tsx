import React from "react";
import PlayerForm from "./components/PlayerForm.tsx";
import GameOverview from "./components/GameOverview.tsx/index.ts";
import Balance from "./components/Balance.tsx/index.ts";
import AdminDashboard from "./components/AdminDashboard.tsx/index.ts";

const App = () => {
  return (
    <div className="App">
      <h1>Dead Pigeons Game</h1>
      <PlayerForm />
      <GameOverview />
      <Balance />
      <AdminDashboard />
    </div>
  );
};

export default App;
