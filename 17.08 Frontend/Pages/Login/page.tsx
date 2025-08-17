import React, { useState } from "react";
import './page.css';
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Login: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    const url = "http://localhost:5111/login";
    const entities = { email, password };

    try {
      const response = await axios.post(url, entities);

      const token = response.data.authToken;

      if (token) {
        localStorage.setItem("token", token);
        setError(null);
        navigate("/game");
      } else {
        setError("No token received from server.");
      }
    } catch (error: any) {
      setError(error.response?.data);
      console.error("Login error:", error);
    }
  };


  return (
    <div className="login-container">
      <div className="login-body">
        
        <div className="login-header">
          <h2 className="login-title">GameX</h2>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="login-inputs">

          <div className="login-input">
            <i className="fas fa-envelope"></i>
            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>

          <div className="login-input">
            <i className="fas fa-lock"></i>
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
        </div>

        <div className="login-submit" onClick={handleSubmit}>
          Login
        </div>
      </div>
    </div>
  );
};

export default Login;
