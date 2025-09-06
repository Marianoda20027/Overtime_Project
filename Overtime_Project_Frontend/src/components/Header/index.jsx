
// Solo usamos una imagen
import horizontalLogo from '../../assets/Logomark_logotype/ArkoseLabs_Horizontal.jpg'; // Asegúrate de que la ruta sea la correcta

const Header = () => {
  return (
    <header className="login-header">
      <div className="logo-container">
        {/* Solo mostramos el logo horizontal */}
        <img src={horizontalLogo} alt="Arkose Labs Horizontal Logo" className="horizontal-logo" />
      </div>
    </header>
  );
};

export default Header;
