import { Button, Card, Form, Input } from 'antd'
import { useLoginForm } from './hooks'
import type { LoginPayload } from './types'

const LoginPage: React.FC = () => {
  const { isLoading, onSubmit } = useLoginForm()

  const handleFinish = (values: LoginPayload) => {
    onSubmit(values)
  }

  return (
    <Card title="Login" style={{ maxWidth: 420, margin: '24px auto' }}>
      <Form layout="vertical" onFinish={handleFinish}>
        <Form.Item label="Email" name="email" rules={[{ required: true, type: 'email' }]}>
          <Input placeholder="usuario@empresa.com" />
        </Form.Item>
        <Form.Item label="Password" name="password" rules={[{ required: true }]}>
          <Input.Password />
        </Form.Item>
        <Button type="primary" htmlType="submit" loading={isLoading} block>
          Ingresar
        </Button>
      </Form>
    </Card>
  )
}

export default LoginPage
