export default function PasswordInputField() {
  return (
    <div
      style={{
        width: "300px",
        display: "flex",
        justifyContent: "space-evenly",
        alignItems: "center",
      }}
    >
      <img
        src="../src/assets/images/password.png"
        alt=""
        style={{ width: "50px", height: "50px" }}
      />
      <input
        type="password"
        placeholder="**********"
        style={{ width: "300px", height: "40px", borderRadius: "10px" }}
      ></input>
    </div>
  );
}
