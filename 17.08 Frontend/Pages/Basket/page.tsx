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
  //const [loading, setLoading] = useState(true);

  // GET /cart
  const loadCart = async () => {
    //setLoading(true);
    try {
      const res = await api.get<Cart>("/cart");
      setCart(res.data);
    } finally {
      //setLoading(false);
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

  // POST /cart/add
  const increaseQuantity = async (gameId: number) => {
    await api.post("/cart/add", { GameId: gameId, Quantity: 1 });
    await loadCart();
  };

  // POST /cart/decrement
  const decreaseQuantity = async (gameId: number) => {
    await api.post("/cart/decrement", { GameId: gameId, Quantity: 1 });
    await loadCart();
  };

  // POST /cart/empty
  const emptyCart = async () => {
    await api.post("/cart/empty");
    await loadCart();
  };

  /*if (loading) {
    return (
      <div className="home-container">
        <h1>Your Basket</h1>
        <p></p>
      </div>
    );
  }*/

  return (
    <div className="home-container">
      <h1>Your Basket</h1>

      {games.length === 0 ? (
        <p>Your basket is empty</p>
      ) : (
        <ul>
          {games.map((game) => {
            const lineTotal = (game.price * game.quantity).toFixed(2);
            return (
              <li key={game.gameId}>
                {game.gameName} --- {game.quantity} × ${game.price.toFixed(2)} = ${lineTotal}
                <button onClick={() => increaseQuantity(game.gameId)}><i className="fa-solid fa-plus" /></button>
                <button onClick={() => decreaseQuantity(game.gameId)}><i className="fa-solid fa-minus" /></button>
              </li>
            );
          })}
        </ul>
      )}

      <h2>Total: ${totalPrice.toFixed(2)}</h2>

      <button disabled={games.length === 0}>Buy</button>
      <button disabled={games.length === 0} onClick={emptyCart}>
        <i className="fa-solid fa-trash" /> Empty
      </button>
    </div>
  );
};

export default Basket;
