import { useNavigate } from "react-router-dom";
import axios from "axios";
import { useEffect, useState } from "react";

const api = axios.create({
  baseURL: "",            
  withCredentials: true,  
});

type UserDetail = {
  name: string;
  surname: string;
  email: string;
  money: number;
};

const EditProfile: React.FC = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState<UserDetail | null>(null);
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [amount, setAmount] = useState<number>(0);

  const fetchUser = async () => {
    try {
      const res = await api.get<UserDetail>("user/getUserInfo");
      setUser(res.data);
      setName(res.data.name);
      setSurname(res.data.surname);
      setEmail(res.data.email);
    } catch {
      console.error();
    }
  };

  useEffect(() => {
    fetchUser();
  }, []);

  const putWithParams = async (url: string, params: Record<string, any>) => {
    try {
      await api.put(url, null, { params });
      await fetchUser();
    } catch {
      console.error();
    }
  };

  return (
    <div className="profile-container">
      <h2>Profile</h2>

      {user && (
        <div>
          <p><strong>Name:</strong> {user.name} {user.surname || "No Surname"}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p><strong>Balance:</strong> ${user.money}</p>
          <button onClick={() => navigate("/profile")}>Go Profile</button>
        </div>
      )}

      <div>
        <div>
          <label>New Name</label>
          <input value={name} onChange={(e) => setName(e.target.value)} />
          <button onClick={() => putWithParams("user/changeName", { name })}>Save Name</button>
        </div>

        <div>
          <label>New Surname</label>
          <input value={surname} onChange={(e) => setSurname(e.target.value)} />
          <button onClick={() => putWithParams("user/changeSurname", { surname })}>Save Surname</button>
        </div>

        <div>
          <label>New Email</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
          <button onClick={() => putWithParams("user/changeEmail", { email })}>Save Email</button>
        </div>

        <div>
          <label>New Password</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
          <button onClick={() => putWithParams("user/changePassword", { password })}>Update Password</button>
        </div>

        <div>
          <label>Add Funds</label>
          <input value={amount} onChange={(e) => setAmount(parseInt(e.target.value || "0"))}/>
          <button onClick={() => putWithParams("user/addFunds", { money: amount })}>Add</button>
        </div>
      </div>
    </div>
  );
};

export default EditProfile;
