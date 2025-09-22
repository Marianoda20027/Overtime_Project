-- Create the users table
CREATE TABLE users (
    user_id UUID PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    address VARCHAR(255),
    phone VARCHAR(50),
    role VARCHAR(20) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    salary DECIMAL(10, 2) NOT NULL
);

-- Create the overtime_requests table
CREATE TABLE overtime_requests (
    overtime_id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    cost_center VARCHAR(255),
    justification TEXT,
    status VARCHAR(20) DEFAULT 'pending',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    cost DECIMAL(10, 2),
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

-- Create the overtime_approvals table
CREATE TABLE overtime_approvals (
    approval_id UUID PRIMARY KEY,
    overtime_id UUID NOT NULL,
    manager_id UUID NOT NULL,
    approved_hours DECIMAL(5, 2) NOT NULL,
    approval_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) DEFAULT 'approved',
    comments TEXT,
    rejection_reason TEXT,
    FOREIGN KEY (overtime_id) REFERENCES overtime_requests(overtime_id),
    FOREIGN KEY (manager_id) REFERENCES users(user_id)
);

-- Create the roles table
CREATE TABLE roles (
    role_id UUID PRIMARY KEY,
    role_name VARCHAR(50) NOT NULL,
    permissions TEXT
);

-- Create the notifications table
CREATE TABLE notifications (
    notification_id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    message TEXT NOT NULL,
    date_sent DATETIME DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) DEFAULT 'sent',
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);
