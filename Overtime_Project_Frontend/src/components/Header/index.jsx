import { Link } from 'react-router-dom';
import horizontalLogo from '../../assets/Logomark_logotype/ArkoseLabs_Horizontal.jpg'; 

const Header = () => {
  return (
    <header className="login-header">
      <div className="logo-container">
        <Link to="/home">
          <img
            src={horizontalLogo}
            alt="Arkose Labs Horizontal Logo"
            className="horizontal-logo"
            style={{ cursor: 'pointer' }} 
          />
        </Link>
      </div>
    </header>
  );
};

export default Header;