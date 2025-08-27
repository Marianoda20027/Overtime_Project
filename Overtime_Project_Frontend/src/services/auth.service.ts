import type { LoginPayload } from '../pages/Login/types'

const API_URL = import.meta.env.VITE_API_URL

export const login = async (payload: LoginPayload) => {
  const res = await fetch(`${API_URL}/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  })
  if (!res.ok) throw new Error('Login failed')
  return res.json()
}
