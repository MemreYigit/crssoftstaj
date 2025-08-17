import {Link, useParams} from "react-router-dom";
import axios from "axios";
import { useEffect, useState } from "react";
import "./page.css"
import downloadImage from "../../Assets/download.jpg";

const SingleGame: React.FC = () => {
  const {gameId} = useParams();
  const [game, setGame] = useState<any>(null);

  useEffect (() => {
    axios.get(`http://localhost:5111/game/${gameId}`)
      .then(response => {
        setGame(response.data)
      }).catch(error => {
        console.log(error)
      })
  }, [gameId]);

  return (
    <div className="singleGame-container">
      {game ? (
        <div className="singleGame-body">
          <div className="singleGame-info">
            <h2>{game.name}</h2>
            <div className="singleGame-PT">
              <h4>Platform: {game.platform}</h4>
              <h4>Type: {game.type}</h4>
            </div>
            <h3>{game.description}</h3>
            <h3>Price: ${game.price}</h3>
          </div>
          <div className="singleGame-img">
            <img src={downloadImage} alt={game.name} className="game-image" />
          </div>
        </div>
        ) : (
          <p>Loading...</p>
        )}
      <Link to="/game">Back to games</Link>
    </div>
  );
};

export default SingleGame;