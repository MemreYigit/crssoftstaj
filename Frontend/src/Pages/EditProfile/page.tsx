import { useNavigate } from "react-router-dom";
import api from "../../Api/api";
import { useEffect, useState } from "react";
import "./page.css";

type UserDetail = {
  name: string;
  surname: string;
  email: string;
  money: number;
};

type EditProfileRequestModel = {
  name: string;
  surname: string;
  email: string;
}

type ChangePasswordRequestModel = {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

const EditProfile: React.FC = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState<UserDetail>();
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [email, setEmail] = useState("");
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [action, setAction] = useState(false);            // Change password part open or close
  const [pMessage, setPMessage] = useState("");           // Change password message
  const [pError, setPError] = useState(false);            // Change password error

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

  const editProfile = async (data: EditProfileRequestModel) => {
    try {
      await api.put("/user/editProfile", data);
      await fetchUser();
    } catch {
      console.error();
    }
  };

  const changePassword = async (data: ChangePasswordRequestModel) => {
    try {
      const res = await api.put("/user/changePassword", data);
      setPMessage(res.data.message);
      setPError(false);
      setCurrentPassword("");
      setConfirmPassword("");
      setNewPassword("");
      setAction(false);
      await fetchUser();
    } catch (err: any) {
      console.error(err);
      if (err.response?.data) {
        setPMessage(err.response.data);
        setPError(true);
      } else {
        setPMessage("Failed to change password");
        setPError(true);
      }    
    }
  };

  const handleSaveProfile = () => {
    const payload: EditProfileRequestModel = { name, surname, email };
    return editProfile(payload);
  };

  const handleChangePassword = () => {
    const payload: ChangePasswordRequestModel = { currentPassword, newPassword, confirmPassword };
    return changePassword(payload);
  };

  return (
    <div className="profile-container">
      <h2>Profile</h2>

      {user && (
        <div className="user-card">
          <p><strong>Name:</strong> {user.name} {user.surname || "No Surname"}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p><strong>Balance:</strong> ${user.money}</p>
          <button onClick={() => navigate("/profile")}>Back to Profile</button>
        </div>
      )}

      <div>
        <div className="editProfile">
          <label>New Name</label>
          <input value={name} onChange={(e) => setName(e.target.value)} />

          <label>New Surname</label>
          <input value={surname} onChange={(e) => setSurname(e.target.value)} />

          <label>New Email</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />

          <button onClick={handleSaveProfile}>Save Changes</button>
        </div>
        
        <button onClick={() => {setAction(prev => !prev); setPMessage("");}}>Change Password</button>

        {!pError && (
          <p style={{ color: "green" }}>{pMessage}</p>
        )}
        
        {action && (
          <div className="passwordChange">
            <label>Current Password</label>
            <input type="password" value={currentPassword} onChange={(e) => setCurrentPassword(e.target.value)} />

            <label>New Password</label>
            <input type="password" value={newPassword} onChange={(e) => setNewPassword(e.target.value)} />

            <label>Confirm Password</label>
            <input type="password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} />

            <button onClick={handleChangePassword}>Save Password Change</button>

            {pError && (
              <p style={{ color: "red" }}>{pMessage}</p>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default EditProfile;
