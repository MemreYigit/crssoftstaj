import './App.css';
import Login from './Pages/Login/page';
import Profile from './Pages/Profile/page';
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
            <Route index element={<Game />} />
            <Route path='login' element={<Login />} />
            <Route path='game/:gameId' element={<SingleGame />} />
            <Route path="search" element={<SearchPage />} />
            <Route path="basket" element={<Basket />} />
            <Route path="profile" element={<Profile />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </CartProvider>
  );
};

export default App;
