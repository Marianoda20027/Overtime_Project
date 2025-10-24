
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
