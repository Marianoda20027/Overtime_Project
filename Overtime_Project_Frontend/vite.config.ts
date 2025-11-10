import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

export default defineConfig({
  plugins: [react()],
  preview: {
    port: parseInt(process.env.PORT || '4173'),
    host: true,
    strictPort: true,
    allowedHosts: [
      'overtimeprojectfrontend-production.up.railway.app',
      '.railway.app'
    ]
  },
  server: {
    port: 5173,
    host: true
  }
})