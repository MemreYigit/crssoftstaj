import React from "react";
import "./page.css"
import { useCart } from "../../Context/CartContext";

const Basket: React.FC = () => {
  const {games, increaseQuantity, decreaseQuantity, emptyCart} = useCart();
  
  const totalPrice = games.reduce((sum, game) => {
    return sum + (game.price * game.quantity);
  }, 0);


  return (
    <div className="home-container">
      <h1>Your Basket</h1>
      
      {games.length === 0 ? (<p>Your basket is empty</p>) : 
      (
        <ul>
          {games.map((game) => (
            <li key={game.id}>
              {game.name} --- Quantity: {game.quantity} x {game.price.toFixed(2)} === {(game.price*game.quantity).toFixed(2)}
              <button><i className="fa-solid fa-plus" onClick={() => increaseQuantity(game.id)}></i></button>
              <button><i className="fa-solid fa-minus" onClick={() => decreaseQuantity(game.id)}></i></button>
            </li>
          ))}
        </ul>
      )
      }

      <h2>Total: ${totalPrice}</h2>
      <button disabled={games.length === 0}>Buy</button>
      <button><i className="fa-solid fa-trash" onClick={() => emptyCart()}></i></button>
    </div>
  );
};

export default Basket;