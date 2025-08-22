import { NavLink } from "react-router-dom"
import "./Navbar.css"
import axios from "axios";
import { useState, useEffect } from "react";

const api = axios.create({
  baseURL: "",            
  withCredentials: true,  
});

const Navbar = () => {  
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const checkAuth = async () => {
      try {
        const response = await api.get('/isAuthenticated');
        setIsAuthenticated(response.data);
      } catch (error) {
        setIsAuthenticated(false);
      }
    };

    checkAuth();
      }, []);

  const handleLogout = () => {
    try {
      api.post("/logout");
      setIsAuthenticated(false);
    }
    catch (err) {
      console.error("Logout failed:", err);
    }
  }


  return (
    <div className="navbar-container">
      <div>
        <h1 className="navbar-title"><NavLink to='/'>GameX</NavLink></h1>
      </div>
      <nav className="navbar">
        <ul>
          {!isAuthenticated ? (
            <li><NavLink to="/login">Login</NavLink></li>
          ) : (
            <li><NavLink to="/" onClick={handleLogout}>Logout</NavLink></li>
          )}
          <li><NavLink to="/game">Games</NavLink></li>
          <li><NavLink to="/basket">Cart ${}</NavLink></li>
        </ul>
      </nav>
    </div>
  );
};

export default Navbar;