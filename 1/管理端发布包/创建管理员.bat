@echo off
chcp 65001 >nul
title 🔐 管理员注册工具

echo ╔═══════════════════════════════════════╗
echo ║   🔐  管理员账户注册工具              ║
echo ╚═══════════════════════════════════════╝
echo.
echo 正在创建管理员账户到数据库...
echo.

mysql -u assess -p < "%~dp0create-admin.sql"

echo.
echo ═══════════════════════════════════════
echo.
echo 完成后在 http://localhost:9527 登录
echo 默认账号: admin  密码: admin888
echo.
pause
