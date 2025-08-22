import React from "react";
import "./page.css"
import axios from "axios";
import { useNavigate } from "react-router-dom";

const api = axios.create({
  baseURL: "",            
  withCredentials: true,  // important for cookie
});

const Home: React.FC = () => {
  const navigate = useNavigate();

  const tryAuth = async () => {
    try {
      await api.get("/isAuthenticated"); 
      navigate("/game");        
    } catch (err: any) {
      if (axios.isAxiosError(err) && err.response?.status === 401) {
        navigate("/login");
      } else {
        console.error(err);
      }
    }
  };


  return (
    <div className="home-container">
      <h1>Home</h1>
      <button onClick={tryAuth}>Go games if you authenticatd!</button>
    </div>
  );
};

export default Home;
