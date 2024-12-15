import React, { useState } from "react";
import styles from "./SelectWinnersBoard.module.css";

interface SelectWinnersBoardProps {
  onConfirmSelection?: (selectedNumbers: number[]) => void;
}

const SelectWinnersBoard: React.FC<SelectWinnersBoardProps> = ({
  onConfirmSelection,
}) => {
  const [selectedNumbers, setSelectedNumbers] = useState<number[]>([]);

  const toggleNumber = (number: number) => {
    if (selectedNumbers.includes(number)) {
      setSelectedNumbers(selectedNumbers.filter((n) => n !== number));
    } else if (selectedNumbers.length < 3) {
      setSelectedNumbers([...selectedNumbers, number]);
    }
  };

  const handleConfirm = () => {
    if (!onConfirmSelection) return;
    if (selectedNumbers.length === 3) {
      onConfirmSelection(selectedNumbers);
      setSelectedNumbers([]); // Reset the selection after confirming
    } else {
      alert("Please select exactly 3 numbers.");
    }
  };

  return (
    <div className={styles.selectWinnersBoard}>
      <div className={styles.numberGrid}>
        {Array.from({ length: 16 }, (_, i) => i + 1).map((number) => (
          <button
            key={number}
            className={`${styles.numberButton} ${
              selectedNumbers.includes(number) ? styles.selected : ""
            }`}
            onClick={() => toggleNumber(number)}
          >
            {number}
          </button>
        ))}
      </div>
      <button className={styles.confirmButton} onClick={handleConfirm}>
        Confirm Winners
      </button>
    </div>
  );
};

export default SelectWinnersBoard;
