
export const decodeJWT = (token) => {
  try {
    if (!token || typeof token !== "string") {
      console.warn("Token inválido o vacío");
      return null;
    }

    const base64Url = token.split('.')[1]; // segunda parte del JWT
    if (!base64Url) {
      console.warn("Formato de JWT inválido");
      return null;
    }

    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );

    return JSON.parse(jsonPayload);
  } catch (error) {
    console.error("Error al decodificar JWT:", error);
    return null;
  }
};
