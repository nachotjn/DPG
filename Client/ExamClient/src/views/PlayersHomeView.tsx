import './PlayersHomeView.css';

const PlayersHomeView = () => {
  return (
    <div className="players-home-container">
      <div className="players-home-header">
        <img src="./src/assets/images/logo.png" alt="Logo" className="logo" />
        <h1>Play 'Dead Pigeons'</h1>
        <div className="players-info">
          <p>Player: Dan Jensen</p>
          <p>Balance: 1000DKK</p>
        </div>
      </div>
      <div className="players-home-body">
        <button className="play-btn">Play a new board</button>
        <button className="upload-btn">
          <span className="upload-icon">↑</span>
          Upload image
        </button>
      </div>
      <div className="players-home-footer">
        <p>&copy;JerneIF</p>
      </div>
            {/* Palomas en el fondo */}
      <img src="./src/assets/images//pigeon.gif" alt="Pigeon 1" className="pigeon" />
      <img src="./src/assets/images//pigeon.gif" alt="Pigeon 2" className="pigeon" />
      <img src="./src/assets/images//pigeon.gif" alt="Pigeon 3" className="pigeon" />

    </div>
  );
}

export default PlayersHomeView;
