import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'; // Usamos Routes y element en lugar de Switch y component
import LoginPage from './pages/Login/page'; // Importamos la página de login
import { ConfigProvider, theme } from 'antd';

export default function App() {
  return (
    <ConfigProvider theme={{ algorithm: theme.defaultAlgorithm }}>
      <Router>
          <Routes>
            {/* Ruta raíz para que el login sea la primera página cargada */}
            <Route path="/" element={<LoginPage />} />
            {/* Otras rutas pueden ser añadidas aquí */}
          </Routes>
      </Router>
    </ConfigProvider>
  );
}
