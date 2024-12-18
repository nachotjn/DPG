import { useState, useEffect } from 'react';
import { fetchTransactionsForPlayer, confirmTransaction } from '../../../services/api';

const PlayerTransactions = ({ playerId }: { playerId: string }) => {
  const [transactions, setTransactions] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadTransactions = async () => {
      try {
        const data = await fetchTransactionsForPlayer(playerId);
        setTransactions(data);
      } catch (err) {
        setError("Failed to load transactions.");
      }
    };

    if (playerId) {
      loadTransactions();
    }
  }, [playerId]);

  const handleConfirmTransaction = async (transactionId: string) => {
    try {
      await confirmTransaction(transactionId, true); // Confirm the transaction
      setTransactions((prevTransactions) =>
        prevTransactions.map((transaction) =>
          transaction.transactionid === transactionId
            ? { ...transaction, isconfirmed: true }
            : transaction
        )
      );
    } catch (error) {
      setError("Failed to confirm the transaction.");
    }
  };

  const renderTransactionStatus = (isConfirmed: boolean) => {
    return isConfirmed ? 'Confirmed' : 'Not confirmed';
  };

  return (
    <div>
      {error && <p>{error}</p>}
      <h2>Transactions for Player {playerId}</h2>
      <ul>
        {transactions.map((transaction) => (
          <li key={transaction.transactionid}>
            <p>Amount: {transaction.amount}</p>
            <p>Type: {transaction.transactiontype}</p>
            <p>Balance After Transaction: {transaction.balanceaftertransaction}</p>
            <p>Description: {transaction.description}</p>
            <p>Status: {renderTransactionStatus(transaction.isconfirmed)}</p>
            {!transaction.isconfirmed && (
              <button onClick={() => handleConfirmTransaction(transaction.transactionid)}>
                Confirm Transaction
              </button>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default PlayerTransactions;
