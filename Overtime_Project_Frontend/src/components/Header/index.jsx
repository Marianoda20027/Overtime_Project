import { Link, useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import horizontalLogo from '../../assets/Logomark_logotype/ArkoseLabs_Horizontal.jpg';

const Header = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    Cookies.remove('jwt'); // Eliminar token
    navigate('/'); // Redirigir a la página de login interna
  };

  return (
    <header
      className="login-header"
      style={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '0 20px',
      }}
    >
      <div
  className="logout"
  style={{
    display: 'flex',
    justifyContent: 'flex-end',  // alinea a la derecha
    alignItems: 'center',        // centra verticalmente respecto al header
    flex: 1,                      // ocupa el espacio restante del header
    height: '100%',               // igual altura que el header
    width: '100%',
    paddingLeft: '10px',          // opcional, separación del logo
  }}
>
 
</div>

      <div className="logo-container">
        <Link to="/home">
          <img
            src={horizontalLogo}
            alt="Horizontal Logo"
            className="horizontal-logo"
            style={{ cursor: 'pointer' }}
          />
        </Link>
      </div>
    </header>
  );
};

export default Header;
