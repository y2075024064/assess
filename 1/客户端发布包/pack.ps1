# ══════════════════════════════════════════
#  📦 客户端打包脚本
#  复制源码 + 启动脚本到发布目录
# ══════════════════════════════════════════

$Root = (Get-Item $PSScriptRoot).Parent.FullName
$Output = "$PSScriptRoot\客户端发布包"

Write-Host "╔═══════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   📦 打包客户端                       ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════╝" -ForegroundColor Cyan

# 清理旧目录
if (Test-Path $Output) { Remove-Item $Output -Recurse -Force }
New-Item -ItemType Directory -Path $Output -Force | Out-Null

# 复制用户前端源码
Write-Host "▶ 复制用户前端 ..." -ForegroundColor Yellow
Copy-Item "$Root\2\assessweb" "$Output\2-用户前端" -Recurse -Exclude "node_modules"
Write-Host "  ✅ 前端" -ForegroundColor Green

# 复制后端 API 源码
Write-Host "▶ 复制后端 API ..." -ForegroundColor Yellow
Copy-Item "$Root\3" "$Output\3-后端API" -Recurse -Exclude "bin","obj","node_modules"
Write-Host "  ✅ 后端" -ForegroundColor Green

# 复制米奈 AI 源码
Write-Host "▶ 复制米奈 AI ..." -ForegroundColor Yellow
Copy-Item "$Root\4" "$Output\4-米奈AI" -Recurse -Exclude "bin","obj"
Write-Host "  ✅ 米奈AI" -ForegroundColor Green

# 复制启动脚本和说明
Copy-Item "$PSScriptRoot\start.bat" "$Output\启动客户端.bat"
Copy-Item "$PSScriptRoot\说明.txt" "$Output\使用说明.txt"
Copy-Item "$Root\1\create-admin.bat" "$Output\创建管理员.bat"
Copy-Item "$Root\1\create-admin.sql" "$Output\create-admin.sql"

Write-Host ""
Write-Host "╔═══════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   ✅ 打包完成！                       ║" -ForegroundColor Green
Write-Host "║   输出: 客户端\客户端发布包\           ║" -ForegroundColor White
Write-Host "║   把这个文件夹压缩发给别人即可         ║" -ForegroundColor White
Write-Host "╚═══════════════════════════════════════╝" -ForegroundColor Cyan
