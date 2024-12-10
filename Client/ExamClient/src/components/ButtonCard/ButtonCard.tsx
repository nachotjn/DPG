import { Link } from "react-router-dom";
import styles from "./buttonCard.module.css";

type IButtonCard = {
  text: string; 
  cssClass: string;
  to: string; 
};

export const ButtonCard = (props: IButtonCard) => {
  return (
    <Link to={props.to} className={styles["button-item"]}>
      <div className={styles["button-background"]}>
        <span className={styles[props.cssClass]}>{props.text}</span>
      </div>
    </Link>
  );
};
