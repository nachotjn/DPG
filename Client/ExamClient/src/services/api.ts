import axios from "axios";

const API_URL = "https://localhost:7218";  

// Login function
export const login = async (email: string, password: string) => {
  try {
    const response = await axios.post(`${API_URL}/api/auth/login`, {
      email,
      password,
    });
    return response.data;  // This will return the token if login is successful
  } catch (error) {
    console.error("Error logging in", error);
    throw error;
  }
};


