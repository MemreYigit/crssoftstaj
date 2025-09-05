import api from "../../Api/api";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./page.css"; 

const Money: React.FC = () => {
  const [amount, setAmount] = useState(0);
  const navigate = useNavigate();

  const addFunds = async () => {
    try {
      await api.put("/user/addFunds", null, {params: { money: amount }});
      navigate("/profile");
    } catch (err) {
      console.error(err);
    }
  }

  return (
    <div className="profile-container">
      <div className="addFunds">
        <label>Add Funds</label>
        <input placeholder="Name" />
        <input placeholder="Credit Card Number" />
        <input placeholder="Expiration Date" />
        <input placeholder="CVC" />
        <input value={amount} onChange={(e) => setAmount(parseInt(e.target.value || "0"))}/>
        <div className="buy-buttons">
          <button onClick={addFunds}>Add</button>
          <button onClick={() => navigate("/profile")}>Back to Profile</button>
        </div>
      </div>
    </div>
  );
};

export default Money;