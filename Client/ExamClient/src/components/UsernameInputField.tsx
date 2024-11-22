export default function UsernameInputField() {
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
        src="../src/assets/images/user.png"
        alt=""
        style={{ width: "50px", height: "50px" }}
      />
      <input
        type="text"
        placeholder="admin@jerneif.dk"
        style={{ width: "200px", height: "40px", borderRadius: "10px" }}
      ></input>
    </div>
  );
}
