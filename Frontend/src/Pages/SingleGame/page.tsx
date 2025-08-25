import {Link, useParams} from "react-router-dom";
import axios from "axios";
import { useEffect, useState } from "react";
import "./page.css"
import downloadImage from "../../Assets/download.jpg";

const api = axios.create({
  baseURL: "",            
  withCredentials: true,  
});

const SingleGame: React.FC = () => {
  const {gameId} = useParams();
  const [game, setGame] = useState<any>(null);
  const [comment, setComment] = useState("");

  useEffect (() => {
    api.get(`/game/${gameId}`)
      .then(response => {
        setGame(response.data)
      }).catch(error => {
        console.log(error)
      })
  }, [gameId]);


  const addComment = async () => {
    if (!comment.trim()) return;
    
    try {
      await api.post(`/comment/add/${gameId}`, { text: comment });
      setComment("");
    } catch (error: any) {
      console.error("Error adding comment:", error);
    }
  };

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
      <div>
        <textarea value={comment} onChange={(e) => setComment(e.target.value)}/>
        <button onClick={addComment}>Add Comment</button>
      </div>
      <Link to="/game">Back to games</Link>
    </div>
  );
};

export default SingleGame;