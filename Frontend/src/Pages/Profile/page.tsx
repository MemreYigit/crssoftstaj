import { useNavigate } from "react-router-dom"
import axios from "axios";
import { useEffect, useState } from "react";
import "./page.css"


const api = axios.create({
  baseURL: "",            
  withCredentials: true,  
});

interface OrderItem {
  orderID: number;
  gameID: number;
  quantity: number;
  price: number;
}

interface Order {
  orderId: number;
  orderDate: string;
  orderPrice: number;
  status: string;
  orderItems: OrderItem[];
}

const Profile: React.FC = () => {
  const navigate = useNavigate();
  const [orders, setOrders] = useState<any[]>([]);

  const handleLogout = async () => {
    try {
      await api.post("/logout");
      navigate("/login");
    }
    catch (err) {
      console.error("Logout failed: ", err);
    }
  }

  useEffect(() => {
    api.get("order/orderDetails")
      .then(res => setOrders(res.data))
      .catch(console.error);
  }, []);


  return (
    <div className="profile-container">
      <h2>Profile</h2>
      
      <div className="orders-section">
        <h3>Your Orders</h3>
        
        {orders.length === 0 ? (
          <p className="no-orders">You don't have any orders!</p>
        ) : (
          <div className="orders-list">
            {orders.map((order: Order) => (
              <div key={order.orderId} className="order-card">
                <div className="order-header">
                  <div className="order-info">
                    <h4>Order #{order.orderId}</h4>
                    <span className="order-date">
                        {new Date(order.orderDate).toLocaleString("en-US", {
                          year: "numeric",
                          month: "short",
                          day: "numeric",
                          hour: "2-digit",
                          minute: "2-digit"
                        })}
                    </span>
                  </div>
                  <div className="order-status">
                    <span className={`status-badge ${order.status.toLowerCase()}`}>{order.status}</span>
                  </div>
                </div>

                <div className="order-items">
                  <h5>Items:</h5>
                  {order.orderItems.map((item: OrderItem) => (
                    <div key={`${item.orderID}-${item.gameID}`} className="order-item">
                      <div className="order-item-info">
                        <span className="order-item-name">{item.gameID}</span>
                        <span className="order-item-quantity">Qty: {item.quantity}</span>
                      </div>
                      <div className="order-item-price">
                        ${item.price}
                      </div>
                    </div>
                  ))}
                </div>

                <div className="order-footer">
                  <div className="order-total">
                    <p>Total: ${order.orderPrice}</p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      <button onClick={handleLogout} className="logout-btn">Logout</button>
    </div>
  );
};


export default Profile;
