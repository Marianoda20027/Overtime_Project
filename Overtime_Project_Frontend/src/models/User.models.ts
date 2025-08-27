export interface User {
  id: string
  name: string
  email: string
  role: 'EMPLOYEE' | 'MANAGER' | 'PEOPLE_OPS' | 'PAYROLL_SME'
}
