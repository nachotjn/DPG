import axios from "axios";

const API_URL = "http://localhost:5070";

export const createBoard = async (selectedNumbers: number[], cost: number) => {
  try {
    const response = await axios.post(`${API_URL}/api/boards`, {
      selectedNumbers,
      cost,
    });
    return response.data;
  } catch (error) {
    console.error("Error creating board", error);
    throw error;
  }
};
