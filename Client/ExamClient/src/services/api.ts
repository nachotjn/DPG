import axios from "axios";

const API_URL = "https://localhost:7218";  

// Login 
export const login = async (email: string, password: string) => {
  try {
    const response = await axios.post(`${API_URL}/api/auth/login`, {
      email,
      password,
    });
    return response.data;  
  } catch (error) {
    console.error("Error logging in", error);
    throw error;
  }
};


// PLAYERS  
export const fetchAllPlayers = async () => {
  try {
    const token = localStorage.getItem("token"); 
    const response = await axios.get(`${API_URL}/api/player`, {
      headers: {
        Authorization: `Bearer ${token}`, 
      },
    });
    const players = response.data?.$values; 
    if (!Array.isArray(players)) {
      console.error("Unexpected data format:", response.data);
      throw new Error("Unexpected data format.");
    }
    return players;
  } catch (error) {
    console.error("Error fetching players", error);
    throw error;
  }
};

export const createPlayer = async (createPlayerDto: { 
  name: string; 
  email: string; 
  phone: string;
  password: string;
  isAdmin:boolean;
  isActive:boolean;
  balance : number; }) => {
  try {
    const token = localStorage.getItem("token"); 
    const response = await axios.post(`${API_URL}/api/player`, createPlayerDto, {
      headers: {
        Authorization: `Bearer ${token}`, 
      },
    });
    return response.data; 
  } catch (error) {
    console.error("Error creating player", error);
    throw error; 
  }
};



// GAMES
export const fetchAllGames = async () => {
  try {
    const token = localStorage.getItem("token"); 
    const response = await axios.get(`${API_URL}/api/game`, {
      headers: {
        Authorization: `Bearer ${token}`, 
      },
    });
    const games= response.data?.$values; 
    if (!Array.isArray(games)) {
      console.error("Unexpected data format:", response.data);
      throw new Error("Unexpected data format.");
    }
    return games;
  } catch (error) {
    console.error("Error fetching games", error);
    throw error;
  }
};


