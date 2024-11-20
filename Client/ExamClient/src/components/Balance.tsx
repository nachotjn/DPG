import React from "react";
import { useAtom } from "jotai";
import { playerBalanceAtom } from "../store/atoms";

const Balance = () => {
  const [balance] = useAtom(playerBalanceAtom);

  return (
    <div>
      <h3>Your Balance: {balance} DKK</h3>
      {/* logica para manejar depositos y actualizaciones de saldo */}
    </div>
  );
};

export default Balance;
