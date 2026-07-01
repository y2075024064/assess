-- Admin account creation script
-- Usage: mysql -u assess -p < create-admin.sql

USE assess;

-- ====== Modify these values ======
SET @account  = 'admin';
SET @password = 'admin888';
SET @phone    = '13800000000';
SET @nickname = 'SuperAdmin';
-- =================================

-- Check if exists
SET @exists = (SELECT COUNT(*) FROM ta_member_account WHERE Account = @account);
SELECT IF(@exists > 0, 'Admin already exists, skipped', 'Creating admin account...') AS 'Result';

-- Create member (Type=5 = Admin)
INSERT INTO ta_member (`Type`, `Gender`, `Nickname`, `MobilePhone`, `IsDelete`, `CreationTime`)
SELECT 5, 0, @nickname, @phone, 0, NOW()
WHERE NOT EXISTS (SELECT 1 FROM ta_member_account WHERE Account = @account);

-- Create login account (EncodeType=0 = plain text)
INSERT INTO ta_member_account (`MemberId`, `Account`, `Password`, `Signature`, `EncodeType`, `CanModifyAccount`)
SELECT m.Id, @account, @password, '', 0, 0
FROM ta_member m
WHERE m.Nickname = @nickname
  AND NOT EXISTS (SELECT 1 FROM ta_member_account WHERE Account = @account);

-- Show result
SELECT '--- Result ---' AS '';
SELECT Id, Nickname, Type FROM ta_member WHERE Nickname = @nickname;
SELECT CONCAT('Account: ', @account) AS 'Info';
SELECT CONCAT('Password: ', @password) AS 'Info';
SELECT CONCAT('Login: http://localhost:9527') AS 'Info';
