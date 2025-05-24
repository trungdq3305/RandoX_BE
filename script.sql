CREATE DATABASE IF NOT EXISTS randox_db;
USE randox_db;

-- 1. WithdrawStatus
CREATE TABLE withdraw_status (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    withdraw_status_name VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 2. TransactionType
CREATE TABLE transaction_type (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    transaction_type_name VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 3. Role
CREATE TABLE role (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    role_name VARCHAR(20) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 4. TransactionStatus
CREATE TABLE transaction_status (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    transaction_status_name VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 5. ShipmentStatus
CREATE TABLE shipment_status (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    shipment_status_name VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 6. Account
CREATE TABLE account (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    email VARCHAR(255) NOT NULL,
    dob DATE NOT NULL,
    password VARCHAR(255) NOT NULL,
    phone_number VARCHAR(15),
    status INT DEFAULT 1,
    role_id VARCHAR(36),
    FOREIGN KEY (role_id) REFERENCES role(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 7. Wallet
CREATE TABLE wallet (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    balance DECIMAL(12,2) NOT NULL DEFAULT 0,
    FOREIGN KEY (id) REFERENCES account(id) ON DELETE CASCADE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);
-- 9. WalletHistory
CREATE TABLE wallet_history (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    time_transaction DATE NOT NULL,
    amount DECIMAL(12,2) NOT NULL,
    account_id VARCHAR(36),
    transaction_type_id VARCHAR(36),
    FOREIGN KEY (account_id) REFERENCES account(id),
    FOREIGN KEY (transaction_type_id) REFERENCES transaction_type(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);
-- 10. Voucher
CREATE TABLE voucher (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    voucher_name VARCHAR(255) NOT NULL,
    voucher_discount_amount DECIMAL(12,2),
    is_discount_percentage BIT DEFAULT 0,
    start_date DATE,
    end_date DATE,
    amount INT,
    min_order_value DECIMAL(12,2),
    max_discount_value DECIMAL(12,2),
    is_active BIT DEFAULT 1,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);
-- 11. Cart
CREATE TABLE cart (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    total_amount DECIMAL(12,2),
    FOREIGN KEY (id) REFERENCES account(id) ON DELETE CASCADE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);
-- 13. Order
CREATE TABLE `order` (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    total_amount DECIMAL(12,2),
    shipping_cost DECIMAL(12,2),
    account_id VARCHAR(36),
    voucher_id VARCHAR(36),
    FOREIGN KEY (account_id) REFERENCES account(id),
    FOREIGN KEY (voucher_id) REFERENCES voucher(id),
    FOREIGN KEY (id) REFERENCES cart(id) ON DELETE CASCADE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);
-- 8. Transaction
CREATE TABLE transaction (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    payment_type_id VARCHAR(36),
    payment_location BIT,
    pay_date DATE,
    amount DECIMAL(12,2),
    Description VARCHAR(255),
    transaction_status_id VARCHAR(36),
    wallet_history_id VARCHAR(36),
    FOREIGN KEY (transaction_status_id) REFERENCES transaction_status(id),
    FOREIGN KEY (wallet_history_id) REFERENCES wallet_history(id),
    FOREIGN KEY (id) REFERENCES `order`(id) ON DELETE CASCADE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 15. Manufacturer
CREATE TABLE manufacturer (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    manufacturer_name VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 18. Promotions
CREATE TABLE promotion (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    event VARCHAR(255),
    start_date DATE,
    end_date DATE,
    percentage_discount_value INT,
    discount_value DECIMAL(12,2),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 17. Product_set
CREATE TABLE product_set (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    product_set_name VARCHAR(255),
    description VARCHAR(255),
    set_quantity INT,
    quantity INT,
    price DECIMAL(12,2),
    promotion_id VARCHAR(36),
    FOREIGN KEY (promotion_id) REFERENCES promotion(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);
-- 16. Product
CREATE TABLE product (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    product_name VARCHAR(255) NOT NULL,
    description VARCHAR(255),
    quantity INT,
    price DECIMAL(12,2) NOT NULL,
    manufacturer_id VARCHAR(36),
    product_set_id VARCHAR(36),
    promotion_id VARCHAR(36),
    FOREIGN KEY (manufacturer_id) REFERENCES manufacturer(id),
    FOREIGN KEY (product_set_id) REFERENCES product_set(id),
    FOREIGN KEY (promotion_id) REFERENCES promotion(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);
-- 12. Cart_Product (many-to-many between Cart and Product)
CREATE TABLE cart_product (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    product_id VARCHAR(36),
    cart_id VARCHAR(36),
    FOREIGN KEY (cart_id) REFERENCES cart(id),
    FOREIGN KEY (product_id) REFERENCES product(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);


-- 14. Address
CREATE TABLE address (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    full_address VARCHAR(255) NOT NULL,
    phone_number VARCHAR(255),
    recipient_name VARCHAR(255),
    is_default BIT DEFAULT 0,
    account_id VARCHAR(36),
    FOREIGN KEY (account_id) REFERENCES account(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 19. Shipment
CREATE TABLE shipment (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    test_result_dispatched_date DATE,
    test_result_delivery_date DATE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 20. ShipmentHistory
CREATE TABLE shipment_history (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    shipment_status_id VARCHAR(36),
    shipment_id VARCHAR(36),
    account_id VARCHAR(36),
    FOREIGN KEY (shipment_status_id) REFERENCES shipment_status(id),
    FOREIGN KEY (shipment_id) REFERENCES shipment(id),
    FOREIGN KEY (account_id) REFERENCES account(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 21. Withdraw
CREATE TABLE with_draw_form (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    title VARCHAR(255) NOT NULL,
    amount DECIMAL(12,2) NOT NULL,
    withdraw_status_id VARCHAR(36),
    account_id VARCHAR(36),
    FOREIGN KEY (withdraw_status_id) REFERENCES withdraw_status(id),
    FOREIGN KEY (account_id) REFERENCES account(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- 22. Image (sản phẩm hoặc đơn hàng)
CREATE TABLE image (
    id VARCHAR(36) PRIMARY KEY DEFAULT (UUID()),
    order_id VARCHAR(36),
    product_id VARCHAR(36),
    image_url VARCHAR(255),
    FOREIGN KEY (order_id) REFERENCES `order`(id),
    FOREIGN KEY (product_id) REFERENCES product(id),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted_at DATETIME NULL,
    is_deleted BIT DEFAULT 0
);

-- WithdrawStatus data
INSERT INTO withdraw_status (withdraw_status_name) VALUES
('Pending'),
('Success'),
('Fail');

-- TransactionType data
INSERT INTO transaction_type (transaction_type_name) VALUES
('Deposit'),
('Payment'),
('Withdrawal');

-- Role data
INSERT INTO role (role_name) VALUES
('Admin'),
('Manager'),
('Staff'),
('Customer');

-- TransactionStatus data
INSERT INTO transaction_status (transaction_status_name) VALUES
('Pending'),
('Fail'),
('Success');

-- ShipmentStatus data
INSERT INTO shipment_status (shipment_status_name) VALUES
('Pending'),
('Shipping'),
('Fail'),
('Success');
