import React, { useState } from "react";
import { createTransaction } from "../../../services/api"; 

interface CreateTransactionFormProps {
  playerId: string;
  playerBalance: number; 
}

const CreateTransactionForm: React.FC<CreateTransactionFormProps> = ({ playerId, playerBalance }) => {
  const [transactionType, setTransactionType] = useState<string>("MobilePay Code");
  const [amount, setAmount] = useState<string>("0");  
  const [description, setDescription] = useState<string>(""); 
  const [isConfirmed] = useState<boolean>(false);  
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const balanceAfterTransaction = playerBalance + (parseFloat(amount) || 0);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const transactionData = {
      playerid: playerId,
      transactiontype: transactionType,
      amount: parseFloat(amount), 
      balanceaftertransaction: balanceAfterTransaction,
      description: transactionType === "MobilePay Code" ? description : "", 
      isconfirmed: isConfirmed,
    };

    try {
      await createTransaction(transactionData); 
      setSuccessMessage("Transaction with MobilePay code created successfully!");
    } catch (error) {
      setError("Failed to create transaction. Please try again.");
    }
  };

  return (
    <div>
      <h2>Create Transaction</h2>
      <form onSubmit={handleSubmit}>
        {/* Transaction Type Selection */}
        <div>
         
        </div>

        {/* Amount */}
        <div>
          <label>Amount:</label>
          <input
            type="text"  
            value={amount}
            onChange={(e) => setAmount(e.target.value.replace(/[^0-9.]/g, ""))}  
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
