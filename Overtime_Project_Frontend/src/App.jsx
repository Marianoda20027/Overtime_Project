import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'; 
import OvertimePage from './pages/Overtime/page';
import LoginPage from './pages/Login/page';
import { ConfigProvider, theme } from 'antd';

export default function App() {
  return (
    <ConfigProvider theme={{ algorithm: theme.defaultAlgorithm }}>
      <Router>
        <Routes>
          {/* Ruta para la página de solicitudes de horas extra */}
          <Route path="/" element={<LoginPage />} />
           <Route path="/overtime" element={<OvertimePage />} />
        </Routes>
      </Router>
    </ConfigProvider>
  );
}
