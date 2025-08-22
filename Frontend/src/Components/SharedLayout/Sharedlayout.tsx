import React from "react";
import { Outlet} from "react-router-dom";
import Navbar from "../Navbar/Navbar";

const Sharedlayout: React.FC = () => {
  return (
    <div>
      <Navbar />
      <Outlet />
    </div>
  );
};

export default Sharedlayout;
