import { useAtom } from "jotai";
import { gameAtom, playerAtom } from "../../../store/atoms";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import style from "./PlayerHomeView.module.css";
import logoImg from "../../../assets/images/logo.png";

const PlayerHomeView = () => {
  const [player] = useAtom(playerAtom); 
  const [game] = useAtom(gameAtom);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const navigate = useNavigate();

  const handlePlayGame = () => {
    navigate("/player-game");  
  };

  const handleUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      setSelectedFile(file);
      alert("Image uploaded successfully!");
      console.log("Uploaded file:", file.name);
    }
  };

  // Get player info from the atom
  const playerName = player?.userName || "Player Name";  
  const playerBalance = player?.balance || 0;  

  return (
    <div className={style["players-home-container"]}>
      <div className={style["players-home-header"]}>
        <img src={logoImg} alt="Logo" className="logo" />
        <h1>Play 'Dead Pigeons'</h1>
        <div className={style["players-info"]}>
          <p><strong>Player:</strong> {playerName.replace(/_/g, ' ')}</p>
          <p><strong>Balance:</strong> {playerBalance} DKK</p>
        </div>
      </div>
      <div className={style["players-home-body"]}>
        <button className={style["play-btn"]} onClick={handlePlayGame}>
          Play a new board
        </button>
        <label className={style["upload-btn"]}>
          <span className={style["upload-icon"]}>↑</span>
          Upload image
          <input
            type="file"
            accept="image/*"
            onChange={handleUpload}
            style={{ display: "none" }}
          />
        </label>
      </div>
      <div className={style["players-home-footer"]}>
        <p>&copy; JerneIF</p>
      </div>
    </div>
  );
};

export default PlayerHomeView;
