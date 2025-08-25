import React, { useEffect, useMemo, useState } from "react";
import axios from "axios";
import "./page.css";

type CartItem = { 
  id: number; 
  gameId: number;
  gameName: string; 
  price: number; 
  quantity: number 
};

type Cart = { 
  cartId: string; 
  items: CartItem[] 
};


const api = axios.create({
  baseURL: "",            
  withCredentials: true,  // important for cookie
});

const Basket: React.FC = () => {
  const [cart, setCart] = useState<Cart>({ cartId: "", items: [] });

  const loadCart = async () => {
    try {
      const res = await api.get<Cart>("/cart");
      setCart(res.data);
    } finally {
    }
  };

  useEffect(() => {
    loadCart();
  }, []);

  const games = cart.items ?? [];
  const totalPrice = useMemo(
    () => (cart.items ?? []).reduce((sum, g) => sum + g.price * g.quantity, 0),
    [cart.items]
  );

  const increaseQuantity = async (gameId: number) => {
    await api.post("/cart/add", { GameId: gameId, Quantity: 1 });
    await loadCart();
  };

  const decreaseQuantity = async (gameId: number) => {
    await api.post("/cart/decrement", { GameId: gameId, Quantity: 1 });
    await loadCart();
  };

  const emptyCart = async () => {
    await api.post("/cart/empty");
    await loadCart();
  };

  const buy = async () => {
    await api.post("/order/createfromcart")
    await loadCart();
  }

  return (
    <div className="basket-container">
      <h1>Your Basket</h1>

      {games.length === 0 ? (
        <p className="empty-basket">Your basket is empty</p>
      ) : (
        <div className="basket-items">
          {games.map((game) => {
            const lineTotal = (game.price * game.quantity).toFixed(2);
            return (
              <div key={game.gameId} className="basket-item">
                <div className="item-details">
                  <h3 className="game-title">{game.gameName}</h3>
                  <div className="price-info">
                    <span className="item-price">${game.price.toFixed(2)}</span>
                    <span className="quantity-label">Quantity: {game.quantity}</span>
                  </div>
                </div>
                <div className="item-controls">
                  <div className="quantity-controls">
                    <button onClick={() => decreaseQuantity(game.gameId)} className="qty-btn minus"><i className="fa-solid fa-minus" /></button>
                    <span className="qty-display">{game.quantity}</span>
                    <button onClick={() => increaseQuantity(game.gameId)} className="qty-btn plus"><i className="fa-solid fa-plus" /></button>
                  </div>
                  <div className="line-total">${lineTotal}</div>
                </div>
              </div>
            );
          })}
        </div>
      )}

      {games.length > 0 && (
        <div className="basket-footer">
          <div className="total-section">
            <h2>Total: ${totalPrice.toFixed(2)}</h2>
          </div>
          <div className="action-buttons">
            <button onClick={buy} className="buy-btn">Buy</button>
            <button onClick={emptyCart} className="empty-btn"><i className="fa-solid fa-trash" /> Empty Basket</button>
          </div>
        </div>
      )}
    </div>
  );
};

export default Basket;
