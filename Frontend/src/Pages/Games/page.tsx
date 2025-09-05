import React, { useEffect, useState } from "react";
import "./page.css";
import api from "../../Api/api";
import { useNavigate } from "react-router-dom";

const Game: React.FC = () => {
  const [games, setGames] = useState<any[]>([]);
  const [search, setSearch] = useState("");
  const [searchResults, setSearchResults] = useState<any[]>([]);
  const [showDropdown, setShowDropdown] = useState(false);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  // Load all games
  useEffect(() => {
    api.get("/game")
      .then(res => setGames(res.data))
      .catch(console.error);
  }, []);

  // Live search for the dropdown
  useEffect(() => {
    if (!search.trim()) {
      setSearchResults([]);
      setShowDropdown(false);
      return;
    }

    setLoading(true);
    const t = setTimeout(() => {              
      api.get(`/game/search?s=${encodeURIComponent(search.trim())}`)
        .then(res => {
          setSearchResults(res.data);
          setShowDropdown(true);
        })
        .catch(console.error)
        .finally(() => setLoading(false));
    }, 250);

    return () => clearTimeout(t);             // Clean up timer (250)
  }, [search]);

  const handleSelect = (id: number) => {
    setShowDropdown(false);
    navigate(`/game/${id}`);
  };

  const handleSearchClick = () => {
    if (!search.trim()) return;
    navigate(`/search?s=${encodeURIComponent(search.trim())}`);
  };

  const addToCart = (gameId: number, qty: number = 1) => {
    try {
      api.post("/cart/add", { GameId: gameId, Quantity: qty });
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="games-container">
      <div className="search-container" style={{ position: "relative" }}>
        <input
          type="text"
          placeholder="Search"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <button type="button" onClick={handleSearchClick}>
          <i className="fa fa-search"></i>
        </button>

        {showDropdown && (
          <div className="search-dropdown">
            {loading && <div className="dropdown-status">Searching…</div>}
            {!loading && searchResults.length === 0 && (
              <div className="dropdown-status">No results</div>
            )}
            {!loading &&
              searchResults.map((g: any) => (
                <div
                  key={g.id}
                  className="dropdown-item"
                  onMouseDown={() => handleSelect(g.id)}
                >
                  <img src={g.imageUrl} alt={g.name} />
                  <div className="dropdown-text">
                    <div className="dropdown-title">{g.name}</div>
                    <div className="dropdown-platform">{g.platform}</div>
                  </div>
                </div>
              ))}
          </div>
        )}
      </div>

      <div className="games-body">
        {games.map((game) => (
          <div key={game.id} onClick={() => navigate(`/game/${game.id}`)} className="game-card">
            <img src={game.imageUrl} alt={game.name} className="game-image" />
            <h2 className="game-name">{game.name}</h2>
            
            <div className="game-info">
              <h3 className="game-platform">{game.platform}</h3>
              <h3 className="game-price">${game.price}</h3>
              <button className="shopping-button" onClick={(e) => {e.stopPropagation(); addToCart(game.id, 1);}}>
                <i className="fa-solid fa-cart-shopping"></i>
              </button>
            </div>
            
          </div>
        ))}
      </div>
    </div>
  );
};

export default Game;