import { useState } from 'react'
import type { LoginPayload } from './types'
import { login } from '../../services/auth.service'

export const useLoginForm = () => {
  const [isLoading, setIsLoading] = useState(false)

  const onSubmit = async (payload: LoginPayload) => {
    try {
      setIsLoading(true)
      await login(payload)
      // TODO: redirigir a dashboard
      console.log('Login OK')
    } catch (err) {
      console.error(err)
      alert('Login failed')
    } finally {
      setIsLoading(false)
    }
  }

  return { isLoading, onSubmit }
}
