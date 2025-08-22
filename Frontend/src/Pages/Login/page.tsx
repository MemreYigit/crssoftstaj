import React, { useState } from "react";
import './page.css';
import axios from "axios";
import { useNavigate } from "react-router-dom";

const api = axios.create({
  baseURL: "",            
  withCredentials: true,  // important for cookie
});


const Login: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      await api.post("/login", { email, password });  
      setError(null);
      navigate("/");
    } catch (err: any) {
      setError(err?.response?.data ?? "Login failed.");
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
