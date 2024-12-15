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



// AUTHORIZATIOn
export const changePassword = async (
  currentPassword: string,
  newPassword: string
) => {
  try {
    const token = localStorage.getItem("token");

    if (!token) {
      throw new Error("Authorization token is missing.");
    }

    const response = await axios.post(
      `${API_URL}/api/auth/change-password`, 
      { currentPassword, newPassword },
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );

    if (response.status === 200) {
      return response.data; 
    } else {
      throw new Error("Failed to change password.");
    }
  } catch (error) {
    console.error("Error changing password", error);
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

export const updatePlayer = async (playerId: string, playerData: any) => {
  const token = localStorage.getItem("token");

  try {
    const response = await axios.put(
      `${API_URL}/api/player/${playerId}`,  
      playerData,
      {
        headers: {
          Authorization: `Bearer ${token}`,  
          "Content-Type": "application/json",  
        },
      }
    );

    if (response.status === 200 || response.status == 204) {
      return response.data;
    } else {
      throw new Error("Failed to update player");
    }
  } catch (error) {
    console.error("Error updating player", error);
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

export const updateGame = async (gameId: string, gameData: any) => {
  const token = localStorage.getItem("token");

  try {
    const response = await axios.put(
      `${API_URL}/api/game/${gameId}`,  
      gameData,
      {
        headers: {
          Authorization: `Bearer ${token}`,  
          "Content-Type": "application/json",  
        },
      }
    );

    if (response.status === 200 || response.status == 204) {
      return response.data;
    } else {
      throw new Error("Failed to update game");
    }
  } catch (error) {
    console.error("Error updating game", error);
    throw error;
  }
};





// BOARDS
export const createBoard = async (boardData: {
  numbers: number[];
  isAutoplay: boolean;
  autoplayWeeks: number;
  playerId: string;
  gameId: string;
}) => {
  try {
    const token = localStorage.getItem("token"); 
    const response = await axios.post(`${API_URL}/api/board`, boardData, {
      headers: {
        Authorization: `Bearer ${token}`, 
      },
    });
    return response.data; // Return the created board data
  } catch (error) {
    console.error("Error creating board", error);
    throw error;
  }
};



//WINNERS
export const determineWinnersForGame = async (gameId: string) => {
  const token = localStorage.getItem("token");

  try {
    const response = await axios.post(
      `${API_URL}/api/winner/games/${gameId}/determineWinners`,  
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,  
          "Content-Type": "application/json",  
        },
      }
    );

    if (response.status === 200) {
      return response.data;  
    } else {
      throw new Error("Failed to determine winners for game");
    }
  } catch (error) {
    console.error("Error determining winners", error);
    throw error;  
  }
};

export const fetchWinnersForGame = async (gameId: string) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.get(`${API_URL}/api/winner/games/${gameId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    const winners = response.data?.$values;

    if (!Array.isArray(winners)) {
      console.error("Unexpected data format:", response.data);
      throw new Error("Unexpected data format.");
    }

    return winners;
  } catch (error) {
    console.error("Error fetching transactions", error);
    throw error;
  }
};


// TRANSACTIONS

export const createTransaction = async (transactionData: {
  playerid: string;
  transactiontype: string;
  amount: number;
  balanceaftertransaction: number;
  description: string;
  isconfirmed: boolean;
}) => {
  try {
    const token = localStorage.getItem("token"); 
    const response = await axios.post(`${API_URL}/api/transaction`, transactionData, {
      headers: {
        Authorization: `Bearer ${token}`, 
      },
    });
    return response.data; 
  } catch (error) {
    console.error("Error creating transaction", error);
    throw error;
  }
};


export const fetchTransactionsForPlayer = async (playerId: string) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.get(`${API_URL}/api/transaction/player/${playerId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    const transactions = response.data?.$values;

    if (!Array.isArray(transactions)) {
      console.error("Unexpected data format:", response.data);
      throw new Error("Unexpected data format.");
    }

    return transactions;
  } catch (error) {
    console.error("Error fetching transactions", error);
    throw error;
  }
};

export const confirmTransaction = async (transactionId: string, isConfirmed: boolean) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.put(
      `${API_URL}/api/transaction/${transactionId}/transactionStatus`,
      { isconfirmed: isConfirmed },
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error("Error confirming transaction", error);
    throw error;
  }
};