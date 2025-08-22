import './App.css';
import Login from './Pages/Login/page';
import Home from './Pages/Home/page';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Game from './Pages/Games/page';
import Sharedlayout from './Components/SharedLayout/Sharedlayout';
import SingleGame from './Pages/SingleGame/page';
import SearchPage from './Pages/SearchGame/page';
import Basket from './Pages/Basket/page'
import { CartProvider } from './Context/CartContext';

function App() {
  return (
    <CartProvider>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<Sharedlayout />}>
            <Route index element={<Home />} />
            <Route path='login' element={<Login />} />
            <Route path='game' element={<Game />} />
            <Route path='game/:gameId' element={<SingleGame />} />
            <Route path="search" element={<SearchPage />} />
            <Route path="basket" element={<Basket />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </CartProvider>
  );
};

export default App;
