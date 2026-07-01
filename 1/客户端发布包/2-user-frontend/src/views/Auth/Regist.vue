<template>
  <div class="regist-page">
    <!-- 重庆特色浮动装饰 -->
    <div class="cq-decor">
      <div class="floating-item f1">🥘</div>
      <div class="floating-item f2">🏔️</div>
      <div class="floating-item f3">🚠</div>
      <div class="floating-item f4">🌉</div>
      <div class="floating-item f5">🏮</div>
      <div class="floating-item f6">🌶️</div>
      <div class="floating-item f7">🐼</div>
      <div class="floating-item f8">🎓</div>
    </div>

    <!-- 顶部山城剪影 -->
    <div class="skyline-area">
      <svg viewBox="0 0 420 80" class="skyline-svg">
        <path d="M0 80 L0 50 Q20 30 40 45 Q55 35 70 42 Q85 25 100 38 Q120 20 140 35 Q155 28 170 32 Q190 15 210 30 Q225 22 240 28 Q260 12 280 25 Q300 18 315 22 Q335 10 350 20 Q365 15 380 18 Q400 8 420 15 L420 80 Z"
          fill="#6A1B9A" opacity="0.12"/>
        <path d="M0 80 L0 55 Q25 40 50 50 Q75 35 100 48 Q130 30 160 45 Q190 28 220 42 Q250 25 280 38 Q310 22 340 36 Q370 20 400 32 Q410 28 420 30 L420 80 Z"
          fill="#6A1B9A" opacity="0.08"/>
      </svg>
    </div>

    <!-- 吉祥物 -->
    <div class="mascot-area">
      <div class="mascot-wrapper">
        <svg viewBox="0 0 160 160" class="mascot-svg">
          <ellipse cx="80" cy="115" rx="42" ry="16" fill="#7B1FA2"/>
          <path d="M38 100 Q38 68 80 62 Q122 68 122 100" fill="#CE93D8" stroke="#7B1FA2" stroke-width="2"/>
          <ellipse cx="80" cy="95" rx="38" ry="11" fill="#E1BEE7" opacity="0.7"/>
          <circle cx="68" cy="88" r="3.5" fill="#F3E5F5" opacity="0.6"/>
          <circle cx="88" cy="84" r="3" fill="#F3E5F5" opacity="0.6"/>
          <circle cx="96" cy="92" r="3" fill="#F3E5F5" opacity="0.6"/>
          <path d="M52 86 Q55 76 62 79 Q59 83 57 86Z" fill="#E53935"/>
          <circle cx="100" cy="82" r="4" fill="#F48FB1" opacity="0.5"/>
          <circle cx="102" cy="80" r="4" fill="#F48FB1" opacity="0.3"/>
          <rect x="32" y="92" width="10" height="5" rx="2.5" fill="#795548" transform="rotate(-20 37 94)"/>
          <rect x="118" y="92" width="10" height="5" rx="2.5" fill="#795548" transform="rotate(20 123 94)"/>
          <path d="M72 58 Q74 48 72 40" fill="none" stroke="#fff" stroke-width="2" opacity="0.4" stroke-linecap="round">
            <animate attributeName="opacity" values="0.4;0.1;0.4" dur="2s" repeatCount="indefinite"/>
          </path>
          <path d="M80 56 Q82 46 80 36" fill="none" stroke="#fff" stroke-width="2.5" opacity="0.3" stroke-linecap="round">
            <animate attributeName="opacity" values="0.3;0.05;0.3" dur="2.5s" repeatCount="indefinite"/>
          </path>
          <circle cx="72" cy="94" r="4.5" fill="#333"/>
          <circle cx="88" cy="94" r="4.5" fill="#333"/>
          <circle cx="73" cy="92" r="1.5" fill="white"/>
          <circle cx="89" cy="92" r="1.5" fill="white"/>
          <path d="M74 99 Q80 104 86 99" fill="none" stroke="#333" stroke-width="2" stroke-linecap="round"/>
          <ellipse cx="62" cy="99" rx="5" ry="3" fill="#F8BBD0" opacity="0.5"/>
          <ellipse cx="98" cy="99" rx="5" ry="3" fill="#F8BBD0" opacity="0.5"/>
        </svg>
        <div class="reg-title">
          <span class="wave">🎉</span> 加入山城小天才，一起成长！
        </div>
      </div>
    </div>

    <!-- 注册表单卡片 -->
    <div class="form-card">
      <div class="cq-badge">🌶️ 重庆 · 青少年人工智能</div>
      <div class="form-title">账号注册</div>

      <van-form class="regist-form" submitOnEnter @submit="regist">
        <van-field
          :rules="[
            { required: true, message: '账户名必填！' },
            { pattern: /^\w+$/, message: '只能由字母、数字、下划线组成' },
            { pattern: /.{6,}/, message: '账户名至少需要6个字符' },
            { validator: checkAccount, trigger: 'onBlur' }
          ]"
          placeholder="账户名（至少6位）"
          v-model="userName"
          class="custom-field"
          :maxlength="32"
          center
        >
          <template #left-icon><span class="field-icon">👤</span></template>
        </van-field>

        <van-field
          :rules="[
            { required: true, message: '登录密码必填！' },
            { pattern: /^[\u0021-\u007e]+$/, message: '只能由字母、数字、特殊字符组成' },
            { pattern: /.{8,}/, message: '密码至少需要8个字符' }
          ]"
          placeholder="登录密码（至少8位）"
          v-model="password"
          :maxlength="20"
          type="password"
          class="custom-field"
          center
        >
          <template #left-icon><span class="field-icon">🔒</span></template>
        </van-field>

        <van-field
          :rules="[
            { required: true, message: '确认密码必填！' },
            { validator: validateRePwd, message: '两次密码不一致' }
          ]"
          placeholder="确认密码"
          v-model="repassword"
          :maxlength="20"
          type="password"
          class="custom-field"
          center
        >
          <template #left-icon><span class="field-icon">🔐</span></template>
        </van-field>

        <van-field
          :rules="[{ required: true, message: '验证码必填！' }]"
          placeholder="验证码"
          v-model="verCode"
          :maxlength="4"
          class="custom-field ver-code-field"
          center
        >
          <template #left-icon><span class="field-icon">🔑</span></template>
          <template #right-icon>
            <img class="ver-code-img" :src="verCodeImg" alt="刷新" @click="getVerCodeImg()" />
          </template>
        </van-field>

        <van-field
          :rules="[
            { required: true, message: '手机号码必填！' },
            { pattern: /^1[\d]{10}/, message: '号码格式错误！' },
            { validator: checkMobilePhone, trigger: 'onBlur' }
          ]"
          placeholder="手机号码"
          v-model="mobilePhone"
          :maxlength="11"
          class="custom-field"
          center
        >
          <template #left-icon><span class="field-icon">📱</span></template>
        </van-field>

        <van-field
          :rules="[{ required: true, message: '用户类型必选！' }]"
          placeholder="选择用户类型"
          v-model="type"
          readonly
          is-link
          class="custom-field"
          @click="showTypeSelect = true"
          center
        >
          <template #left-icon><span class="field-icon">👥</span></template>
        </van-field>

        <van-field
          v-if="type === '学生'"
          :rules="[{ required: true, message: '当前年级必选！' }]"
          placeholder="选择当前年级"
          v-model="grade"
          readonly
          is-link
          class="custom-field"
          @click="showGradeSelect = true"
          center
        >
          <template #left-icon><span class="field-icon">📚</span></template>
        </van-field>

        <div class="agree-row">
          <van-checkbox v-model="agree" icon-size="16" shape="square" checked-color="#7B1FA2">
            <span class="agree-text">我已阅读并同意</span>
          </van-checkbox>
          <router-link to="/agreement" class="protocol-link">《注册协议》</router-link>
        </div>

        <van-button class="regist-btn" nativeType="submit" type="primary" :disabled="!agree" square block>
          <template #default>
            <span class="btn-text">注册</span>
          </template>
        </van-button>

        <div class="login-link-row">
          已有账号？<router-link to="/login" class="login-link">去登录</router-link>
        </div>
      </van-form>
    </div>

    <van-popup v-model:show="showGradeSelect" position="bottom" round>
      <van-picker :columns="gradeList" @confirm="onGradeSelected" @cancel="showGradeSelect = false" />
    </van-popup>
    <van-popup v-model:show="showTypeSelect" position="bottom" round>
      <van-picker :columns="typeList" @confirm="onTypeSelected" @cancel="showTypeSelect = false" />
    </van-popup>
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify } from 'vant'
import { ref, onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'

const router = useRouter()

const typeList = [
  { text: '普通', value: '普通' },
  { text: '学生', value: '学生' },
  { text: '家长', value: '家长' },
  { text: '教师', value: '教师' }
]

const showGradeSelect = ref(false)
const showTypeSelect = ref(false)
const gradeList = ref<any[]>([])
const mobilePhone = ref('')
const verCodeImg = ref('')
const repassword = ref('')
const password = ref('')
const userName = ref('')
const verCode = ref('')
const grade = ref('')
const type = ref('')
const agree = ref(false)

const validateRePwd = (value: string, rule: any) => {
  return value === password.value
}

// 异步检查账号是否已被使用
const checkAccount = (value: string): Promise<boolean | string> => {
  return new Promise((resolve) => {
    if (!value || value.length < 6) {
      resolve(true) // 长度不足时不触发API检查，交给其他规则
      return
    }
    http
      .get('Auth/CheckAccount', { params: { account: value } })
      .right((res) => {
        resolve(true) // 账号可用
      })
      .error((err) => {
        resolve(err.Msg || '该账户名已被使用')
      })
      .catch(() => {
        resolve(true) // 网络错误时放行，让后端最终校验
      })
  })
}

// 异步检查手机号是否已被绑定
const checkMobilePhone = (value: string): Promise<boolean | string> => {
  return new Promise((resolve) => {
    if (!value || value.length < 11) {
      resolve(true)
      return
    }
    http
      .get('Auth/CheckMobilePhone', { params: { mobilePhone: value } })
      .right((res) => {
        resolve(true) // 手机号可用
      })
      .error((err) => {
        resolve(err.Msg || '该手机号已被绑定')
      })
      .catch(() => {
        resolve(true)
      })
  })
}

const getVerCodeImg = () => {
  http.getFile('Auth/VerCode?codeType=Regist&bgb=true').right((res) => {
    var blob = new Blob([res])
    verCodeImg.value = URL.createObjectURL(blob)
  })
}

const onTypeSelected = (item: any) => {
  if (item.selectedValues.length > 0) {
    type.value = item.selectedValues[0]
    if (type.value === '学生' && gradeList.value.length <= 0) {
      http.get('api/Grade/UsedList').right((res) => {
        for (var i = 0; i < res.Data.length; i++) {
          var item = res.Data[i]
          gradeList.value.push({ text: item, value: item })
        }
      })
    }
  }
  showTypeSelect.value = false
}

const onGradeSelected = (item: any) => {
  if (item.selectedValues.length > 0) {
    grade.value = item.selectedValues[0]
  }
  showGradeSelect.value = false
}

const regist = () => {
  var data = {
    MobilePhone: mobilePhone.value,
    RePassword: repassword.value,
    Password: password.value,
    Account: userName.value,
    MemberType: type.value,
    VerCode: verCode.value,
    Grade: grade.value
  }
  http
    .post('Auth/Regist', data)
    .right((res) => {
      showNotify({
        message: res.Msg,
        type: 'success',
        duration: 2000
      })
      router.push('/login')
    })
    .error((err) => {
      getVerCodeImg()
      showNotify({
        message: err.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

onMounted(() => {
  getVerCodeImg()
})
</script>

<style lang="scss" scoped>
.regist-page {
  @apply w-full h-full flex flex-col bg-gradient-to-b from-[#f3e5f5] via-[#e8eaf6] to-[#fce4ec] relative overflow-hidden;
}

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
  font-size: 24px;
  filter: drop-shadow(0 2px 4px rgba(0,0,0,0.08));
}

.f1 { top: 6%; left: 4%; animation-delay: 0s; }
.f2 { top: 9%; left: 20%; animation-delay: 0.8s; }
.f3 { top: 12%; left: 38%; animation-delay: 0.4s; }
.f4 { top: 5%; left: 55%; animation-delay: 1.6s; }
.f5 { top: 11%; left: 72%; animation-delay: 1.2s; font-size: 22px; }
.f6 { top: 4%; left: 85%; animation-delay: 2s; font-size: 20px; }
.f7 { top: 15%; left: 60%; animation-delay: 2.4s; font-size: 20px; }
.f8 { top: 8%; left: 46%; animation-delay: 2.8s; font-size: 22px; }

@keyframes floatUp {
  0%, 100% { transform: translateY(0) rotate(0deg); }
  50% { transform: translateY(-12px) rotate(5deg); }
}

.skyline-area {
  @apply w-full relative z-10 mt-1;
  height: 45px;
}

.skyline-svg {
  @apply w-full h-full;
}

.mascot-area {
  @apply flex justify-center items-center relative z-10 -mt-4;
}

.mascot-wrapper {
  @apply flex flex-col items-center;
}

.mascot-svg {
  width: 110px;
  height: 110px;
  animation: bounce 2.5s ease-in-out infinite;
  filter: drop-shadow(0 4px 12px rgba(123, 31, 162, 0.15));
}

@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

.reg-title {
  @apply text-base font-bold -mt-1;
  color: #4A148C;
  font-family: 'Microsoft YaHei', sans-serif;

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

.form-card {
  @apply flex-1 mx-4 px-4 pt-4 pb-6 rounded-t-3xl relative z-10 overflow-y-auto;
  background: rgba(255, 255, 255, 0.93);
  backdrop-filter: blur(12px);
  box-shadow: 0 -6px 30px rgba(123, 31, 162, 0.06);
}

.cq-badge {
  @apply text-xs text-center mb-0.5;
  color: #7B1FA2;
  letter-spacing: 1px;
  opacity: 0.7;
}

.form-title {
  @apply text-center text-xl font-bold mb-3;
  color: #37474F;
  font-family: 'Microsoft YaHei', sans-serif;
  letter-spacing: 1px;
}

.regist-form {
  @apply flex flex-col;
}

.custom-field {
  --van-cell-vertical-padding: 10px;
  --van-cell-horizontal-padding: 14px;
  --van-cell-background: #f3e5f5;
  --van-cell-border-color: transparent;
  --van-font-size-md: 14px;
  @apply rounded-2xl mb-2.5 border-none;

  :deep(.van-field__body) {
    @apply items-center;
  }

  :deep(input) {
    @apply text-sm;
    color: #37474F;
  }

  :deep(input::placeholder) {
    color: #b39ddb;
    font-size: 14px;
  }
}

.ver-code-field {
  :deep(.van-field__right-icon) {
    @apply ml-2;
  }
}

.field-icon {
  font-size: 18px;
  margin-right: 4px;
}

.ver-code-img {
  width: 72px;
  height: 28px;
  border-radius: 8px;
  cursor: pointer;
}

.agree-row {
  @apply flex items-center mt-3 mb-4;
  font-size: 13px;

  .agree-text {
    color: #78909C;
  }
}

.protocol-link {
  @apply text-sm no-underline ml-0.5;
  color: #7B1FA2;
  font-weight: 500;

  &:hover { color: #4A148C; }
}

.regist-btn {
  --van-button-primary-background: linear-gradient(135deg, #AB47BC, #7B1FA2);
  --van-button-primary-border-color: transparent;
  @apply rounded-2xl h-[46px] shadow-lg;
  box-shadow: 0 4px 20px rgba(123, 31, 162, 0.3);
}

.btn-text {
  @apply text-base font-bold;
  color: white;
  letter-spacing: 2px;
}

.login-link-row {
  @apply text-center mt-4 text-sm;
  color: #78909C;

  .login-link {
    @apply no-underline;
    color: #7B1FA2;
    font-weight: 500;
    &:hover { color: #4A148C; }
  }
}
</style>
