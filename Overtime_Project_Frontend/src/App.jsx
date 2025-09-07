import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'; 
import OvertimePage from './pages/Overtime/page';
import LoginPage from './pages/Login/page';
import Menu from './pages/Menu/page';
import Request from './pages/Request';
import { ConfigProvider, theme } from 'antd';

export default function App() {
  return (
    <ConfigProvider theme={{ algorithm: theme.defaultAlgorithm }}>
      <Router>
        <Routes>
          {/* Ruta para la página de solicitudes de horas extra */}
          <Route path="/" element={<LoginPage />} />
           <Route path="/Overtime-Request" element={<OvertimePage />} />
            <Route path="/Menu" element={<Menu />} />
                        <Route path="/Request" element={<Request />} />

        </Routes>
      </Router>
    </ConfigProvider>
  );
}
