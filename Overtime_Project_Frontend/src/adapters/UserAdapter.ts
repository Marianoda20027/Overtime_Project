import type { User } from '../models/User.models'

export const userFromApi = (apiUser: any): User => ({
  id: String(apiUser.id),
  name: apiUser.name ?? '',
  email: apiUser.email ?? '',
  role: apiUser.role ?? 'EMPLOYEE',
})
