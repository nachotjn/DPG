import React, { useState } from "react";
import { createPlayer } from "../../../services/api"; // Import the API call

const CreatePlayerForm = ({ refreshPlayers }: any) => {
  // State for form fields
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [password, setPassword] = useState("");
  const [isAdmin, setIsAdmin] = useState(false);
  const [isActive, setIsActive] = useState(true);
  const [balance, setBalance] = useState(0);
  
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // this is to replace spaces with underscores in the name before sending it to the API since players can not have spaces on the db
    const formattedName = name.replace(/\s+/g, "_");

    const playerData = {
      name: formattedName, // Use the formatted name
      email,
      phone,
      password,
      isAdmin,
      isActive,
      balance
    };

    try {
      const player = await createPlayer(playerData);
      setSuccessMessage(`Player ${player.name} created successfully!`);
      
      setName("");
      setEmail("");
      setPhone("");
      setPassword("");
      setIsAdmin(false);
      setIsActive(true);
      setBalance(0);

     
      refreshPlayers();
    } catch (error) {
      setError("Failed to create player. Please try again.");
    }
  };

  return (
    <div>
      <h2>Create New Player</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Name:</label>
          <input 
            type="text" 
            value={name} 
            onChange={(e) => setName(e.target.value)} 
            required 
          />
        </div>

        <div>
          <label>Email:</label>
          <input 
            type="email" 
            value={email} 
            onChange={(e) => setEmail(e.target.value)} 
            required 
          />
        </div>

        <div>
          <label>Phone:</label>
          <input 
            type="text" 
            value={phone} 
            onChange={(e) => setPhone(e.target.value)} 
            required 
          />
        </div>

        <div>
          <label>Password:</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
          />
        </div>

        <div>
          <label>Is Admin:</label>
          <input 
            type="checkbox" 
            checked={isAdmin} 
            onChange={(e) => setIsAdmin(e.target.checked)} 
          />
        </div>

        <div>
          <label>Is Active:</label>
          <input 
            type="checkbox" 
            checked={isActive} 
            onChange={(e) => setIsActive(e.target.checked)} 
          />
        </div>

        <div>
          <label>Balance:</label>
          <input 
            type="number" 
            value={balance} 
            onChange={(e) => setBalance(Number(e.target.value))} 
            required 
            min="0" 
          />
        </div>

        <button type="submit">Create Player</button>
      </form>

      {successMessage && <p style={{ color: "green" }}>{successMessage}</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
  );
};

export default CreatePlayerForm;
