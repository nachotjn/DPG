import Board from "../components/Board";
import GameOverview from "../components/GameOverview";
import Balance from "../components/Balance"
import AdminDashboard from "../components/AdminDashboard"


export default function AdminView() {
  return (
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
        <img
          src="../src/assets/images/logo.png"
          alt="Logo Image"
          style={{ width: "50px", height: "50px" }}
        />
        <Board/>
        <GameOverview/>
        <AdminDashboard/>
        <Balance/>

        <button >SELECT WEEKLY NUMBERS</button>
      </div>
    </section>
  );
}
