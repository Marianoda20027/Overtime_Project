// src/components/Login/types.js

// Definimos los tipos de las propiedades que se esperan en el componente Login
export const LoginPayload = {
  username: String,
  password: String,
};

export const LoginResponse = {
  token: String,
  user: {
    id: String,
    username: String,
  },
};
