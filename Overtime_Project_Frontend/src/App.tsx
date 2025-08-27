import LoginPage from './pages/Login'
import { ConfigProvider, theme } from 'antd'

export default function App() {
  return (
    <ConfigProvider theme={{algorithm: theme.defaultAlgorithm}}>
      <main style={{maxWidth: 960, margin: '40px auto', padding: 16}}>
        <h1>Overtime</h1>
        <LoginPage />
      </main>
    </ConfigProvider>
  )
}
