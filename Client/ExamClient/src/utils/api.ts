import axios from "axios";
import { Player } from "../types/game";

const API_URL = "https://localhost:7218;http://localhost:5070";

// obtener jugadores
export const fetchPlayers = async (): Promise<Player[]> => {
  try {
    const response = await axios.get(`${API_URL}/players`);
    return response.data;
  } catch (error) {
    console.error("Error fetching players:", error);
    throw error;
  }
};

// crear un nuevo jugador
export const createPlayer = async (playerData: Player): Promise<Player> => {
  try {
    const response = await axios.post(`${API_URL}/players`, playerData);
    return response.data;
  } catch (error) {
    console.error("Error creating player:", error);
    throw error;
  }
};

// obtener los numeros ganadores del juego
export const fetchWinningNumbers = async (): Promise<number[]> => {
  try {
    const response = await axios.get(`${API_URL}/winningNumbers`);
    return response.data;
  } catch (error) {
    console.error("Error fetching winning numbers:", error);
    throw error;
  }
};
