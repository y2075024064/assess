<template>
  <div class="login-page">
    <!-- 重庆特色浮动装饰 -->
    <div class="cq-decor">
      <div class="floating-item f1">🥘</div>
      <div class="floating-item f2">🏔️</div>
      <div class="floating-item f3">🌉</div>
      <div class="floating-item f4">🚠</div>
      <div class="floating-item f5">🏛️</div>
      <div class="floating-item f6">🌶️</div>
      <div class="floating-item f7">🏮</div>
      <div class="floating-item f8">🐼</div>
    </div>

    <!-- 顶部山城剪影 -->
    <div class="skyline-area">
      <svg viewBox="0 0 420 80" class="skyline-svg">
        <path d="M0 80 L0 50 Q20 30 40 45 Q55 35 70 42 Q85 25 100 38 Q120 20 140 35 Q155 28 170 32 Q190 15 210 30 Q225 22 240 28 Q260 12 280 25 Q300 18 315 22 Q335 10 350 20 Q365 15 380 18 Q400 8 420 15 L420 80 Z"
          fill="#FF6F00" opacity="0.15"/>
        <path d="M0 80 L0 55 Q25 40 50 50 Q75 35 100 48 Q130 30 160 45 Q190 28 220 42 Q250 25 280 38 Q310 22 340 36 Q370 20 400 32 Q410 28 420 30 L420 80 Z"
          fill="#FF6F00" opacity="0.1"/>
      </svg>
    </div>

    <!-- 吉祥物 - 重庆火锅小精灵 -->
    <div class="mascot-area">
      <div class="mascot-wrapper">
        <svg viewBox="0 0 160 160" class="mascot-svg">
          <!-- 火锅锅体 -->
          <ellipse cx="80" cy="115" rx="45" ry="18" fill="#D84315"/>
          <path d="M35 100 Q35 65 80 60 Q125 65 125 100" fill="#FF7043" stroke="#D84315" stroke-width="2"/>
          <!-- 火锅汤底 -->
          <ellipse cx="80" cy="95" rx="40" ry="12" fill="#FF5252" opacity="0.6"/>
          <!-- 气泡 -->
          <circle cx="65" cy="88" r="4" fill="#FF8A80" opacity="0.7"/>
          <circle cx="85" cy="82" r="3" fill="#FF8A80" opacity="0.7"/>
          <circle cx="95" cy="90" r="3.5" fill="#FF8A80" opacity="0.7"/>
          <!-- 辣椒 -->
          <path d="M55 85 Q58 75 65 78 Q62 82 60 85Z" fill="#F44336"/>
          <path d="M100 80 Q103 70 110 73 Q107 77 105 80Z" fill="#F44336"/>
          <!-- 花椒 -->
          <circle cx="73" cy="92" r="2.5" fill="#5D4037" opacity="0.6"/>
          <circle cx="90" cy="86" r="2" fill="#5D4037" opacity="0.6"/>
          <!-- 锅柄 -->
          <rect x="30" y="90" width="12" height="6" rx="3" fill="#8D6E63" transform="rotate(-20 36 93)"/>
          <rect x="118" y="90" width="12" height="6" rx="3" fill="#8D6E63" transform="rotate(20 124 93)"/>
          <!-- 蒸汽动画 -->
          <path d="M72 58 Q74 48 72 40" fill="none" stroke="#fff" stroke-width="2" opacity="0.4" stroke-linecap="round">
            <animate attributeName="opacity" values="0.4;0.1;0.4" dur="2s" repeatCount="indefinite"/>
          </path>
          <path d="M80 56 Q82 46 80 36" fill="none" stroke="#fff" stroke-width="2.5" opacity="0.3" stroke-linecap="round">
            <animate attributeName="opacity" values="0.3;0.05;0.3" dur="2.5s" repeatCount="indefinite"/>
          </path>
          <path d="M88 58 Q90 48 88 40" fill="none" stroke="#fff" stroke-width="2" opacity="0.4" stroke-linecap="round">
            <animate attributeName="opacity" values="0.4;0.1;0.4" dur="1.8s" repeatCount="indefinite"/>
          </path>
          <!-- 笑脸 -->
          <circle cx="72" cy="95" r="5" fill="#333"/>
          <circle cx="88" cy="95" r="5" fill="#333"/>
          <circle cx="73" cy="93" r="1.8" fill="white"/>
          <circle cx="89" cy="93" r="1.8" fill="white"/>
          <path d="M73 100 Q80 106 87 100" fill="none" stroke="#333" stroke-width="2" stroke-linecap="round"/>
          <!-- 腮红 -->
          <ellipse cx="62" cy="100" rx="6" ry="3" fill="#FF9EAA" opacity="0.6"/>
          <ellipse cx="98" cy="100" rx="6" ry="3" fill="#FF9EAA" opacity="0.6"/>
        </svg>
        <div class="welcome-text">
          <span class="wave">🥘</span> 勒是雾都！欢迎回来～
        </div>
      </div>
    </div>

    <!-- 登录表单 -->
    <div class="form-card">
      <div class="cq-badge">🏔️ 山城 · 重庆</div>
      <div class="form-title">青少年人工智能科普题库</div>

      <van-form class="login-form" submitOnEnter @submit="login">
        <van-field
          :rules="[{ required: true, message: '请输入账户名' }]"
          placeholder="账户名 / 手机号 / 邮箱"
          autocomplete="username"
          v-model="userName"
          class="custom-field"
          :maxlength="32"
          center
        >
          <template #left-icon><span class="field-icon">👤</span></template>
        </van-field>

        <van-field
          :rules="[{ required: true, message: '请输入密码' }]"
          autocomplete="current-password"
          placeholder="登录密码"
          v-model="password"
          class="custom-field"
          :maxlength="32"
          type="password"
          center
        >
          <template #left-icon><span class="field-icon">🔒</span></template>
        </van-field>

        <div class="options-row">
          <van-checkbox v-model="remember" shape="square" checked-color="#D84315">
            <span class="remember-text">记住我</span>
          </van-checkbox>
          <router-link to="/regist" class="forgot-link">没有账号？<span class="cq-highlight">去注册</span></router-link>
        </div>

        <van-button class="login-btn" nativeType="submit" type="primary" square block>
          <template #default>
            <span class="btn-text">登录</span>
          </template>
        </van-button>
      </van-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify } from 'vant'
import { ref } from 'vue'
import { useAppStore } from '@store/app'
import { useUserStore } from '@store/user'
import { RouterLink, useRouter } from 'vue-router'

const router = useRouter()

const userName = ref('')
const password = ref('')
const remember = ref(false)

const login = () => {
  var data = {
    Account: userName.value,
    Password: password.value,
    Remember: remember.value
  }
  http
    .post('Auth/Login', data)
    .right((res) => {
      var appStore = useAppStore()
      var userStore = useUserStore()
      userStore.setUserInfo({
        name: res.Data.NickName || res.Data.Account,
        type: res.Data.MemberType,
        gender: res.Data.Gender,
        avatar: res.Data.Avatar
      })
      appStore.setToken(res.Data.Token, remember.value)
      router.push('/')
    })
    .error((err) => {
      password.value = ''
      showNotify({
        message: err.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}
</script>

<style lang="scss" scoped>
.login-page {
  @apply w-full h-full flex flex-col bg-gradient-to-b from-[#ffebee] via-[#fff8e1] to-[#fff3e0] relative overflow-hidden;
}

/* ===== 重庆特色浮动装饰 ===== */
.cq-decor {
  position: absolute;
  width: 100%;
  height: 100%;
  pointer-events: none;
  z-index: 0;
}

.floating-item {
  position: absolute;
  animation: floatUp 4s ease-in-out infinite;
  font-size: 26px;
  filter: drop-shadow(0 2px 4px rgba(0,0,0,0.1));
}

.f1 { top: 6%; left: 5%; animation-delay: 0s; }
.f2 { top: 10%; left: 22%; animation-delay: 0.8s; }
.f3 { top: 4%; left: 42%; animation-delay: 1.6s; }
.f4 { top: 12%; left: 62%; animation-delay: 0.4s; }
.f5 { top: 7%; left: 78%; animation-delay: 2s; }
.f6 { top: 16%; left: 35%; animation-delay: 2.4s; font-size: 20px; }
.f7 { top: 3%; left: 58%; animation-delay: 1.2s; font-size: 22px; }
.f8 { top: 14%; left: 88%; animation-delay: 2.8s; font-size: 22px; }

@keyframes floatUp {
  0%, 100% { transform: translateY(0) rotate(0deg); }
  50% { transform: translateY(-14px) rotate(6deg); }
}

/* ===== 山城剪影 ===== */
.skyline-area {
  @apply w-full relative z-10 mt-2;
  height: 50px;
}

.skyline-svg {
  @apply w-full h-full;
}

/* ===== 吉祥物 ===== */
.mascot-area {
  @apply flex justify-center items-center relative z-10 -mt-4;
}

.mascot-wrapper {
  @apply flex flex-col items-center;
}

.mascot-svg {
  width: 120px;
  height: 120px;
  animation: bounce 2.5s ease-in-out infinite;
  filter: drop-shadow(0 4px 12px rgba(216, 67, 21, 0.2));
}

@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

.welcome-text {
  @apply text-lg font-bold -mt-2;
  color: #BF360C;
  font-family: 'Microsoft YaHei', sans-serif;
  text-shadow: 0 1px 2px rgba(0,0,0,0.05);

  .wave {
    display: inline-block;
    animation: wave 1.5s ease-in-out infinite;
  }
}

@keyframes wave {
  0%, 100% { transform: rotate(0deg); }
  25% { transform: rotate(-20deg); }
  75% { transform: rotate(20deg); }
}

/* ===== 表单卡片 ===== */
.form-card {
  @apply flex-1 mx-5 px-5 pt-5 pb-8 rounded-t-3xl relative z-10;
  background: rgba(255, 255, 255, 0.93);
  backdrop-filter: blur(12px);
  box-shadow: 0 -6px 30px rgba(211, 47, 47, 0.08);
}

.cq-badge {
  @apply text-xs text-center mb-1;
  color: #D84315;
  letter-spacing: 2px;
  opacity: 0.7;
}

.form-title {
  @apply text-center text-xl font-bold mb-5;
  color: #37474F;
  font-family: 'Microsoft YaHei', sans-serif;
  letter-spacing: 1px;
}

.login-form {
  @apply flex flex-col;
}

.custom-field {
  --van-cell-vertical-padding: 12px;
  --van-cell-horizontal-padding: 16px;
  --van-cell-background: #fbe9e7;
  --van-cell-border-color: transparent;
  @apply rounded-2xl mb-3 border-none;

  :deep(.van-field__body) {
    @apply items-center;
  }

  :deep(input) {
    @apply text-base;
    color: #37474F;
  }

  :deep(input::placeholder) {
    color: #bcaaa4;
  }
}

.field-icon {
  font-size: 20px;
  margin-right: 6px;
}

.options-row {
  @apply flex justify-between items-center mt-1 mb-5;
}

.remember-text {
  @apply text-sm;
  color: #8D6E63;
}

.forgot-link {
  @apply text-sm no-underline;
  color: #8D6E63;

  .cq-highlight {
    color: #D84315;
    font-weight: 600;

    &:hover {
      color: #BF360C;
    }
  }
}

.login-btn {
  --van-button-primary-background: linear-gradient(135deg, #FF7043, #D84315);
  --van-button-primary-border-color: transparent;
  @apply rounded-2xl h-[48px] shadow-lg;
  box-shadow: 0 4px 20px rgba(216, 67, 21, 0.35);
}

.btn-text {
  @apply text-base font-bold;
  color: white;
  letter-spacing: 2px;
}
</style>
