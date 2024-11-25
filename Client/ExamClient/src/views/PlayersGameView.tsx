import Board from "../components/Board";
import Balance from "../components/Balance"
import PlayerForm from "../components/PlayerForm"

export default function PlayerGameView() {
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
          style={{ 
            width: "50px", 
            height: "50px",
            flexDirection: "column",
            
         }}
        />
        
        <Board/>
        <Balance/>
        <PlayerForm/>

        <button>PLAY SELECTED NUMBERS</button>
      </div>
    </section>
    
  );
}
