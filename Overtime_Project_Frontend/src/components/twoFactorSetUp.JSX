// src/components/TwoFactorSetup.jsx
import { useState, useEffect } from "react";
import QRCode from "qrcode.react";
import axios from "axios";

export default function TwoFactorSetup({ username }) {
  const [secret, setSecret] = useState("");

  useEffect(() => {
    // Pedimos al backend un secreto TOTP para el usuario
    axios.post("/api/auth/register-2fa", username)
      .then(res => setSecret(res.data.secret))
      .catch(err => console.log(err));
  }, [username]);

  if (!secret) return <p>Cargando 2FA...</p>;

  return (
    <div style={{ background: "#e8f5e9", padding: 20, borderRadius: 8 }}>
      <h2>Escanea este código en tu Authenticator</h2>
      <QRCode value={`otpauth://totp/Overtime:${username}?secret=${secret}&issuer=Overtime`} />
      <p>Si no puedes escanear, usa este código manualmente: <b>{secret}</b></p>
    </div>
  );
}
