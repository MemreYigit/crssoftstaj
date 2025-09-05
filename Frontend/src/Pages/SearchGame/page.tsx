import React, { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import api from "../../Api/api";

const SearchPage: React.FC = () => {
  const [searchParams] = useSearchParams();
  const query = searchParams.get("s");
  const [results, setResults] = useState<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (query) {
      api
        .get(`/game/search?s=${encodeURIComponent(query)}`)
        .then(res => setResults(res.data))
        .catch(console.error);
    }
  }, [query]);

  return (
    <div className="games-container">
      <h2>Search results for "{query}"</h2>
      <div className="games-body">
        {results.map((game) => (
          <div key={game.id} onClick={() => navigate(`/game/${game.id}`)} className="game-card">
            <img src={game.imageUrl} alt={game.name} className="game-image" />
            <h2 className="game-name">{game.name}</h2>
            
            <div className="game-info">
              <h3 className="game-platform">{game.platform}</h3>
              <h3 className="game-price">${game.price}</h3>
            </div>

          </div>
        ))}
      </div>
    </div>
  );
};

export default SearchPage;