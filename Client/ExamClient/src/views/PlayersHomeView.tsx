import { useNavigate } from "react-router-dom";
import { useState } from "react";
import "./PlayersHomeView.css";

const PlayersHomeView = () => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const navigate = useNavigate();

  const handlePlayGame = () => {
    navigate("/game");
  };

  const handleUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      setSelectedFile(file);
      alert("Image uploaded successfully!");
      console.log("Uploaded file:", file.name);
    }
  };

  return (
    <div className="players-home-container">
      <div className="players-home-header">
        <img src="./src/assets/images/logo.png" alt="Logo" className="logo" />
        <h1>Play 'Dead Pigeons'</h1>
        <div className="players-info">
          <p>Player: Dan Jensen</p>
          <p>Balance: 1000DKK</p>
          <p>Boards: 3</p>
        </div>
      </div>
      <div className="players-home-body">
        <button className="play-btn" onClick={handlePlayGame}>
          Play a new board
        </button>
        <label className="upload-btn">
          <span className="upload-icon">↑</span>
          Upload image
          <input
            type="file"
            accept="image/*"
            onChange={handleUpload}
            style={{ display: "none" }}
          />
        </label>
      </div>
      <div className="players-home-footer">
        <p>&copy;JerneIF</p>
      </div>
      
      {/* Palomas animadas en el fondo */}
      <img
        src="./src/assets/images/pigeon.gif"
        alt="Pigeon 1"
        className="pigeon pigeon-top"
      />
      <img
        src="./src/assets/images/pigeon.gif"
        alt="Pigeon 2"
        className="pigeon pigeon-middle"
      />
      <img
        src="./src/assets/images/pigeon.gif"
        alt="Pigeon 3"
        className="pigeon pigeon-bottom"
      />
    </div>
  );
};

export default PlayersHomeView;
