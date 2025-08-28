import { Link, useParams } from "react-router-dom";
import axios from "axios";
import { useEffect, useState } from "react";
import "./page.css";
import downloadImage from "../../Assets/download.jpg";

const api = axios.create({
  baseURL: "",            
  withCredentials: true,
});

type Comment = {
  id: number;
  commentText: string;
  createdAt: string;   
  gameID: number;
  userID: number;
  userName: string;
};

const SingleGame: React.FC = () => {
  const { gameId } = useParams();
  const [game, setGame] = useState<any>(null);
  const [makeComment, setMakeComment] = useState("");
  const [comments, setComments] = useState<Comment[]>([]);
  const [error, setError] = useState<string | null>(null);

  const fetchComments = async () => {
    try {
      const res = await api.get(`/comment/${gameId}`);
      setComments(res.data);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    api.get(`/game/${gameId}`)
      .then(r => setGame(r.data))
      .catch(console.error);

    fetchComments();
  }, ); 

  const addComment = async () => {
    if (!makeComment.trim()) return;

    try {
      await api.post(`/comment/add/${gameId}`, { text: makeComment });
      setMakeComment("");
      fetchComments();
    } catch (error) {
      setError("Please login!")
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

      <div className="comments-section">

        {error && <div className="error-message">{error}</div>}

        <div className="comment-add">
          <textarea value={makeComment} onChange={(e) => setMakeComment(e.target.value)} placeholder="Write a comment..."/>
          <button onClick={addComment}><i className="fa-solid fa-paper-plane"></i></button>
        </div>

        <h3>Comments</h3>
        {comments.length > 0 ? (
          comments.map((c) => (
            <div className="comment" key={c.id}>
              <p>
                <strong>{c.userName}:</strong> {c.commentText}
              </p>
              <small>
                {new Date(c.createdAt).toLocaleString("en-US", {
                  year: "numeric",
                  month: "short",
                  day: "numeric",
                  hour: "2-digit",
                  minute: "2-digit",
                })}
              </small>
            </div>
          ))
        ) : (
          <p>No comments yet.</p>
        )}
      </div>

      <Link to="/" className="backtogames">Back to Games</Link>
    </div>
  );
};

export default SingleGame;
