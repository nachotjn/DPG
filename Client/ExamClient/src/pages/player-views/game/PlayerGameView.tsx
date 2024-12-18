import React, { useEffect, useState } from "react";
import { getGameDetails } from "../../../services/api"; // Ajusta la ruta según tu estructura
import styles from "./playerGameView.module.css"; // Importa el CSS modular
import ButtonCard from "../../../components/Board/ButtonCard";

interface GameDetails {
  id: number;
  name: string;
  status: string;
  winningNumbers: number[];
}

const PlayerGameView: React.FC = () => {
  const [gameDetails, setGameDetails] = useState<GameDetails | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchGameDetails = async () => {
      try {
        setLoading(true);
        const response = await getGameDetails(); // Llamada al backend
        setGameDetails(response);
      } catch (err) {
        setError("Error fetching game details. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchGameDetails();
  }, []);

  return (
    <div className={styles.container}>
      {loading && <p className={styles.loading}>Loading game details...</p>}
      {error && <p className={styles.error}>{error}</p>}
      {gameDetails && (
        <div className={styles.gameDetails}>
          <h1 className={styles.title}>{gameDetails.name}</h1>
          <p className={styles.status}>Status: {gameDetails.status}</p>
          <div className={styles.winningNumbers}>
            <h3>Winning Numbers</h3>
            <div className={styles.numbersGrid}>
              {gameDetails.winningNumbers.map((num, index) => (
                <ButtonCard key={index} number={num} />
              ))}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default PlayerGameView;
