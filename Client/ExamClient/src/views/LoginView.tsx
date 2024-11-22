import UsernameInputField from "../components/UsernameInputField";
import PasswordInputField from "../components/PasswordInputField";

export default function LogIn() {
  return (
    <section
      style={{
        width: "100vw",
        height: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        flexDirection: "column",
      }}
    >
      <div
        style={{
          height: "60vh",
          display: "flex",
          justifyContent: "space-evenly",
          alignItems: "center",
          flexDirection: "column",
        }}
      >
        <img
          src="../src/assets/images/logo.png"
          alt="Logo Image"
          style={{ width: "200px", height: "200px" }}
        />
        <UsernameInputField />
        <PasswordInputField />

        <button>Log in</button>
      </div>
    </section>
  );
}
