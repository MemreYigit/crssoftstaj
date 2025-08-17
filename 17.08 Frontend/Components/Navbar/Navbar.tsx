import { NavLink } from "react-router-dom"
import "./Navbar.css"


const Navbar = () => {
  return (
    <div className="navbar-container">
      <div>
        <h1 className="navbar-title"><NavLink to='/'>GameX</NavLink></h1>
      </div>
      <nav className="navbar">
        <ul>
          <li><NavLink to='/login'>Login</NavLink></li>
          <li><NavLink to='/game'>Games</NavLink></li>
          <li><NavLink to='/basket'>Cart ${}</NavLink></li>
        </ul>
      </nav>
    </div>
  );
};

export default Navbar;