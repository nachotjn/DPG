import React, { useState } from "react";
import { createTransaction } from "../../../services/api"; 

interface CreateTransactionFormProps {
  playerId: string;
}

const CreateTransactionForm: React.FC<CreateTransactionFormProps> = ({ playerId }) => {
  const [transactionType, setTransactionType] = useState<string>("Screenshot");
  const [amount, setAmount] = useState<number>(0);
  const [balanceAfterTransaction, setBalanceAfterTransaction] = useState<number>(0);
  const [description, setDescription] = useState<string>(""); 
  const [isConfirmed] = useState<boolean>(false);  
  const [screenshot, setScreenshot] = useState<File | null>(null); 
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (transactionType === "Screenshot" && !screenshot) {
      setError("Please upload a screenshot.");
      return;
    }

    const transactionData = {
      playerid: playerId,
      transactiontype: transactionType,
      amount,
      balanceaftertransaction: balanceAfterTransaction,
      description: transactionType === "MobilePay Code" ? description : "", 
      isconfirmed: isConfirmed,
    };

    try {
      if (transactionType === "Screenshot" && screenshot) {
        // Need to add screenshot handling here

        
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
            type="number"
            value={amount}
            onChange={(e) => setAmount(Number(e.target.value))}
            required
            min="0"
          />
        </div>

        {/* Balance After Transaction */}
        <div>
          <label>Balance After Transaction:</label>
          <input
            type="number"
            value={balanceAfterTransaction}
            onChange={(e) => setBalanceAfterTransaction(Number(e.target.value))}
            required
            min="0"
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
