import { Link, useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import horizontalLogo from '../../assets/Logomark_logotype/ArkoseLabs_Horizontal.jpg';

const Header = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    Cookies.remove('jwt'); 
    navigate('/'); 
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
    justifyContent: 'flex-end',  
    alignItems: 'center',        
    flex: 1,                      
    height: '100%',               
    width: '100%',
    paddingLeft: '10px',          
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
