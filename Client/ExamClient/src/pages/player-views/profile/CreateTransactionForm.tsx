import React, { useState } from "react";
import { createTransaction } from "../../../services/api"; 

interface CreateTransactionFormProps {
  playerId: string;
  playerBalance: number; // Added playerBalance as a prop
}

const CreateTransactionForm: React.FC<CreateTransactionFormProps> = ({ playerId, playerBalance }) => {
  const [transactionType, setTransactionType] = useState<string>("Screenshot");
  const [amount, setAmount] = useState<string>("0");  // Amount is now a string
  const [description, setDescription] = useState<string>(""); 
  const [isConfirmed] = useState<boolean>(false);  
  const [screenshot, setScreenshot] = useState<File | null>(null); 
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  // Calculate balance after transaction dynamically
  const balanceAfterTransaction = playerBalance + (parseFloat(amount) || 0);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (transactionType === "Screenshot" && !screenshot) {
      setError("Please upload a screenshot.");
      return;
    }

    const transactionData = {
      playerid: playerId,
      transactiontype: transactionType,
      amount: parseFloat(amount), // Convert amount to a number
      balanceaftertransaction: balanceAfterTransaction, // Use the dynamically calculated value
      description: transactionType === "MobilePay Code" ? description : "", 
      isconfirmed: isConfirmed,
    };

    try {
      if (transactionType === "Screenshot" && screenshot) {
        // Need to add screenshot handling here (e.g., send to the backend)
      } else {
        await createTransaction(transactionData); 
        setSuccessMessage("Transaction with MobilePay code created successfully!");
      }
    } catch (error) {
      setError("Failed to create transaction. Please try again.");
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setScreenshot(e.target.files[0]); 
    }
  };

  return (
    <div>
      <h2>Create Transaction</h2>
      <form onSubmit={handleSubmit}>
        {/* Transaction Type Selection */}
        <div>
          <label>
            <input
              type="radio"
              value="Screenshot"
              checked={transactionType === "Screenshot"}
              onChange={() => setTransactionType("Screenshot")}
            />
            Screenshot
          </label>
          <label>
            <input
              type="radio"
              value="MobilePay Code"
              checked={transactionType === "MobilePay Code"}
              onChange={() => setTransactionType("MobilePay Code")}
            />
            MobilePay Code
          </label>
        </div>

        {/* Amount */}
        <div>
          <label>Amount:</label>
          <input
            type="text"  // Change this to text input to allow direct editing of the number
            value={amount}
            onChange={(e) => setAmount(e.target.value.replace(/[^0-9.]/g, ""))}  // Allow only numbers and decimal point
            required
          />
        </div>

        {/* Description (for MobilePay Code only) */}
        {transactionType === "MobilePay Code" && (
          <div>
            <label>Code:</label>
            <input
              type="text"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              required
            />
          </div>
        )}

        {/* Screenshot Upload (for Screenshot type only) */}
        {transactionType === "Screenshot" && (
          <div>
            <label>Upload Screenshot:</label>
            <input type="file" accept="image/*" onChange={handleFileChange} required />
          </div>
        )}

        {/* Submit Button */}
        <button type="submit">Create Transaction</button>
      </form>

      {/* Success/Error Messages */}
      {successMessage && <p style={{ color: "green" }}>{successMessage}</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
  );
};

export default CreateTransactionForm;
