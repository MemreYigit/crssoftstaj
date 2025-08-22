import { createContext, ReactNode, useContext, useState } from 'react';


// Shape of an item
export interface CartItem {
  id: number;
  quantity: number;
  name: string;
  price: number;
}

// Shape of the context 
interface CartContextType {
  games: CartItem[];
  addToCart: (id: number, name: string, price: number) => void;
  increaseQuantity: (id: number) => void;
  decreaseQuantity: (id: number) => void;
  removeFromCart: (id: number) => void;
  emptyCart: () => void;
}

// Create template for my backpack
const CartContext = createContext<CartContextType>({
  games: [],
  addToCart: () => {},
  increaseQuantity: () => {},
  decreaseQuantity: () => {},
  removeFromCart: () => {},
  emptyCart: () => {}
});

// Backpack with my staffs
export function CartProvider({children} : {children: ReactNode}) {
  const [games, setItems] = useState<CartItem[]>([]);

  const addToCart = (id: number, name:string, price: number) => {
    setItems(prevItems => {
      const existItem = prevItems.find(item => item.id === id);
      
      if (existItem) {
        return prevItems.map(item =>
          item.id === id ? {...item, quantity: item.quantity + 1} : item
        );
      }
      else {
        return [...prevItems, {id, name, price, quantity:1}]
      }
    });
  };



  const increaseQuantity = (id: number) => {
    setItems(prevItems => {
        return prevItems.map(item => 
          item.id === id ? {...item, quantity: item.quantity + 1} : item
        )});
  };

  const decreaseQuantity = (id: number) => {
    setItems(prevItems => {
      const result: CartItem[] = [];
      for (const item of prevItems) {
        if (item.id !== id) {
          result.push(item);
        }
        else if (item.quantity > 1) {
          result.push({...item, quantity: item.quantity - 1});
        }
        else {
          // do nothing
        }
      }
      return result;
    });
  };

  const removeFromCart = (id: number) => {
    setItems(prevItems => prevItems.filter(item => item.id !== id));
  };

  const emptyCart = () => {
    setItems([]);
  };

  return (
    <CartContext.Provider value={{games, addToCart, increaseQuantity, decreaseQuantity, removeFromCart, emptyCart}}>
      {children}
    </CartContext.Provider>
  );
}

// Reach into backpack
export function useCart() {
  return useContext(CartContext);
}