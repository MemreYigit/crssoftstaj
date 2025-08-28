import React, { useState } from "react";
import "./page.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const api = axios.create({
  baseURL: "",
  withCredentials: true,
});

type Action = "login" | "register";

const Login: React.FC = () => {
  const [action, setAction] = useState<Action>("login");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  const loginSubmit = async () => {
    await api.post("/login", { email, password });
    setError(null);
    navigate("/");
  };

  const registerSubmit = async () => {
    await api.post("/register", { email, password, name });
    setError(null);
    setAction("login");
    navigate("/login");
  };

  const handleSubmit = async (e:React.FormEvent) => {
    try {
      e.preventDefault();
      if (action === "login") {
        await loginSubmit();
      } 
      else {
        await registerSubmit();
      }
    } 
    catch (err: any) {
      setError(err.response.data);
    }
  };

  return (
    <div className="login-container">
      <div className="login-body">
        <div className="login-header">
          <h2 className="login-title">GameX</h2>
        </div>

        {error && <div className="error-message">{error}</div>}

        <form className="login-inputs" onSubmit={handleSubmit}>
          {action === "register" && (
            <div className="login-input">
              <i className="fa-solid fa-circle-user"/>
              <input
                type="text"
                placeholder="Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            </div>
          )}

          <div className="login-input">
            <i className="fas fa-envelope"/>
            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>

          <div className="login-input">
            <i className="fas fa-lock"/>
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
        </form>

        <form className="login-form" onSubmit={handleSubmit}>
          <button className="login-submit">
            {action === "login" ? "Login" : "Register"}
          </button>

          <div className="login-toggle">
            {action === "login" && (
              <button onClick={() => {setAction("register"); setError(null)}}>
                Don’t have an account?
              </button>
            )}
            {action === "register" && (
              <button onClick={() => {setAction("login"); setError(null)}}>
                Already have an account? 
              </button>
            )}

          </div>
        </form>

      </div>
    </div>
  );
};

export default Login;
