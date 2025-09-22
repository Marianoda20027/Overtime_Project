import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import OvertimePage from './pages/Overtime/page';
import LoginPage from './pages/Login/page';
import Menu from './pages/Menu/page';
import Request from './pages/Request';
import ApprovalPage from './pages/Approval';

import { ConfigProvider, theme } from 'antd';

export default function App() {
  // Configuración de tema personalizada para forzar fondo blanco
  const customTheme = {
    algorithm: theme.defaultAlgorithm,
    token: {
      // Colores de fondo principales
      colorBgBase: '#ffffff',
      colorBgContainer: '#ffffff',
      colorBgElevated: '#ffffff',
      colorBgLayout: '#ffffff',
      colorBgSpotlight: '#ffffff',
      colorBgMask: 'rgba(0, 0, 0, 0.45)',
      
      // Colores de texto
      colorText: '#333333',
      colorTextBase: '#333333',
      
      // Color primario (tu verde)
      colorPrimary: '#50B95D',
      
      // Bordes
      colorBorder: '#e9ecef',
      colorBorderSecondary: '#f0f0f0',
    },
    components: {
      // Configuración específica para componentes
      Layout: {
        bodyBg: '#ffffff',
        headerBg: '#ffffff',
        footerBg: '#ffffff',
        siderBg: '#ffffff',
      },
      Button: {
        colorBgContainer: '#ffffff',
      },
      Card: {
        colorBgContainer: '#ffffff',
      },
      Modal: {
        contentBg: '#ffffff',
        headerBg: '#ffffff',
      },
      Drawer: {
        colorBgElevated: '#ffffff',
      },
      Dropdown: {
        colorBgElevated: '#ffffff',
      },
      Menu: {
        itemBg: '#ffffff',
        subMenuItemBg: '#ffffff',
      },
      Table: {
        colorBgContainer: '#ffffff',
        headerBg: '#fafafa',
      },
      Form: {
        itemBg: '#ffffff',
      },
      Input: {
        colorBgContainer: '#ffffff',
      },
      Select: {
        colorBgContainer: '#ffffff',
      },
    }
  };

  return (
    <ConfigProvider theme={customTheme}>
      <div style={{ 
        backgroundColor: '#ffffff', 
        minHeight: '100vh',
        fontFamily: 'Open Sans, sans-serif'
      }}>
        <Router>
          <Routes>
            {/* Ruta para la página de solicitudes de horas extra */}
            <Route path="/" element={<LoginPage />} />
            <Route path="/Overtime-Request" element={<OvertimePage />} />
            <Route path="/home" element={<Menu />} />
            <Route path="/Request" element={<Request />} />
            <Route path="/Approval" element={<ApprovalPage />} />
          </Routes>
        </Router>
      </div>
    </ConfigProvider>
  );
}