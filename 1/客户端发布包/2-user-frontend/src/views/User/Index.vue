<template>
  <!-- 顶部渐变背景 -->
  <div class="profile-header-bg">
    <div class="profile-header-pattern"></div>
  </div>

  <!-- 个人信息卡片 -->
  <div class="profile-card">
    <div class="profile-avatar-row">
      <van-image class="profile-avatar" :src="userInfo.Avatar" fit="cover" height="64" width="64" round />
      <div class="profile-info">
        <div class="profile-name">{{ userInfo.Nickname || '未设置昵称' }}</div>
        <div class="profile-account">账户：{{ userInfo.Account }}</div>
      </div>
      <div class="profile-gender">
        <Man v-if="userInfo.Gender == '男'" class="text-blue-400 w-6 h-6" />
        <Women v-else-if="userInfo.Gender == '女'" class="text-pink-300 w-6 h-6" />
        <EyeClosed v-else class="text-gray-400 w-5 h-5" />
      </div>
    </div>
  </div>

  <!-- 功能菜单 -->
  <div class="menu-section">
    <div class="menu-group">
      <div class="menu-item" @click="gotoExamineLetter()">
        <div class="menu-icon-box" style="background: linear-gradient(135deg, #667eea, #764ba2);">
          <span class="menu-emoji">📋</span>
        </div>
        <span class="menu-label">审核信件</span>
        <van-icon name="arrow" class="menu-arrow" />
      </div>

      <div class="menu-item" @click="gotoMyRace()">
        <div class="menu-icon-box" style="background: linear-gradient(135deg, #f093fb, #f5576c);">
          <span class="menu-emoji">🏆</span>
        </div>
        <span class="menu-label">我的比赛</span>
        <van-icon name="arrow" class="menu-arrow" />
      </div>

      <div class="menu-item" @click="gotoEditUser()">
        <div class="menu-icon-box" style="background: linear-gradient(135deg, #4facfe, #00f2fe);">
          <span class="menu-emoji">✏️</span>
        </div>
        <span class="menu-label">编辑信息</span>
        <van-icon name="arrow" class="menu-arrow" />
      </div>

      <div class="menu-item" @click="showPwdDialog = true">
        <div class="menu-icon-box" style="background: linear-gradient(135deg, #ff6b6b, #ee5a24);">
          <span class="menu-emoji">🔑</span>
        </div>
        <span class="menu-label">修改密码</span>
        <van-icon name="arrow" class="menu-arrow" />
      </div>
    </div>

    <div class="menu-group">
      <div class="menu-item" @click="router.push('/about')">
        <div class="menu-icon-box" style="background: linear-gradient(135deg, #43e97b, #38f9d7);">
          <span class="menu-emoji">ℹ️</span>
        </div>
        <span class="menu-label">关于我们</span>
        <van-icon name="arrow" class="menu-arrow" />
      </div>

      <div class="menu-item" @click="router.push('/contact')">
        <div class="menu-icon-box" style="background: linear-gradient(135deg, #fa709a, #fee140);">
          <span class="menu-emoji">📞</span>
        </div>
        <span class="menu-label">联系客服</span>
        <van-icon name="arrow" class="menu-arrow" />
      </div>
    </div>

    <!-- 退出登录 -->
    <div class="menu-group">
      <div class="menu-item logout-item" @click="handleLogout">
        <div class="menu-icon-box" style="background: linear-gradient(135deg, #e74c3c, #c0392b);">
          <span class="menu-emoji">🚪</span>
        </div>
        <span class="menu-label logout-label">退出登录</span>
        <van-icon name="arrow" class="menu-arrow" />
      </div>
    </div>
  </div>

  <!-- 修改密码弹窗 -->
  <van-dialog
    v-model:show="showPwdDialog"
    title="修改密码"
    show-cancel-button
    @confirm="handleChangePwd"
    confirm-button-color="#667eea"
  >
    <div class="px-4 py-3">
      <van-field v-model="pwdOld" type="password" placeholder="请输入旧密码" class="pwd-field" />
      <van-field v-model="pwdNew" type="password" placeholder="请输入新密码（至少8位）" :maxlength="20" class="pwd-field" />
      <van-field v-model="pwdConfirm" type="password" placeholder="确认新密码" :maxlength="20" class="pwd-field" />
      <van-field v-model="pwdVerCode" placeholder="验证码" :maxlength="4" class="pwd-field">
        <template #right-icon>
          <img class="ver-code-img" :src="pwdVerCodeImg" alt="刷新" @click="getPwdVerCode()" />
        </template>
      </van-field>
    </div>
  </van-dialog>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify, showConfirmDialog } from 'vant'
import { useRouter } from 'vue-router'
import { ref, onBeforeMount } from 'vue'
import { UserInfo } from '@/types/UserInfo'
import { useAppStore } from '@store/app'

import EyeClosed from '@/svgs/icons/eyeClosed.vue'
import Women from '@/svgs/icons/women.vue'
import Man from '@/svgs/icons/man.vue'

const router = useRouter()
const appStore = useAppStore()
const userInfo = ref<UserInfo>(new UserInfo())

// 修改密码
const showPwdDialog = ref(false)
const pwdOld = ref('')
const pwdNew = ref('')
const pwdConfirm = ref('')
const pwdVerCode = ref('')
const pwdVerCodeImg = ref('')

const getPwdVerCode = () => {
  http.getFile('Auth/VerCode?codeType=RePassword&bgb=false').right((res) => {
    var blob = new Blob([res])
    pwdVerCodeImg.value = URL.createObjectURL(blob)
  })
}

const handleChangePwd = () => {
  if (!pwdOld.value) { showNotify({ message: '请输入旧密码', type: 'warning' }); return }
  if (!pwdNew.value || pwdNew.value.length < 8) { showNotify({ message: '新密码至少8位', type: 'warning' }); return }
  if (pwdNew.value !== pwdConfirm.value) { showNotify({ message: '两次密码不一致', type: 'warning' }); return }
  if (!pwdVerCode.value) { showNotify({ message: '请输入验证码', type: 'warning' }); return }

  http.post('Auth/Repwd', {
    OldPwd: pwdOld.value,
    NewPwd: pwdNew.value,
    VerCode: pwdVerCode.value
  }).right((res) => {
    showNotify({ message: '密码修改成功！', type: 'success', duration: 2000 })
    showPwdDialog.value = false
    pwdOld.value = ''
    pwdNew.value = ''
    pwdConfirm.value = ''
    pwdVerCode.value = ''
  }).error((err) => {
    getPwdVerCode()
    showNotify({ message: err.Msg, type: 'danger', duration: 2000 })
  })
}

/** 退出登录 */
const handleLogout = () => {
  showConfirmDialog({
    title: '提示',
    message: '确定要退出登录吗？',
    confirmButtonColor: '#e74c3c'
  }).then(() => {
    appStore.clearToken()
    router.push('/login')
  }).catch(() => {})
}

/**获取用户信息 */
const getUserInfo = () => {
  http.get('api/Member/Info').right((res) => {
    userInfo.value = res.Data
  }).error((res) => {
    showNotify({ message: res.Msg, type: 'danger', duration: 2000 })
  })
}

const gotoExamineLetter = () => { router.push({ name: 'examineLetter' }) }
const gotoMyRace = () => { router.push({ name: 'allRace' }) }
const gotoEditUser = () => {
  var params = userInfo.value
  router.push({ name: 'editUser', state: params })
}

onBeforeMount(() => { getUserInfo() })
</script>

<style scoped>
.profile-header-bg {
  height: 160px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  position: relative;
  overflow: hidden;
}

.profile-header-pattern {
  position: absolute;
  width: 200%; height: 200%; top: -50%; left: -50%;
  background: radial-gradient(circle at 30% 40%, rgba(255,255,255,0.1) 0%, transparent 50%),
              radial-gradient(circle at 70% 60%, rgba(255,255,255,0.08) 0%, transparent 50%);
}

.profile-card {
  margin: -50px 16px 0;
  background: rgba(255,255,255,0.95);
  backdrop-filter: blur(10px);
  border-radius: 20px;
  padding: 20px;
  box-shadow: 0 4px 24px rgba(102,126,234,0.15);
  position: relative; z-index: 2;
}

.profile-avatar-row { display: flex; align-items: center; gap: 14px; }
.profile-avatar { flex-shrink: 0; border: 3px solid #fff; box-shadow: 0 2px 12px rgba(0,0,0,0.1); }
.profile-info { flex: 1; min-width: 0; }
.profile-name { font-size: 18px; font-weight: 700; color: #333; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.profile-account { font-size: 13px; color: #999; margin-top: 4px; }
.profile-gender { flex-shrink: 0; display: flex; align-items: center; justify-content: center; width: 32px; height: 32px; }

.menu-section { padding: 16px; display: flex; flex-direction: column; gap: 12px; }
.menu-group { background: rgba(255,255,255,0.92); border-radius: 16px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.04); }
.menu-item { display: flex; align-items: center; padding: 14px 16px; cursor: pointer; transition: all 0.2s; border-bottom: 1px solid #f5f5f5; }
.menu-item:last-child { border-bottom: none; }
.menu-item:active { background: #f8f9ff; }
.menu-icon-box { width: 36px; height: 36px; border-radius: 12px; display: flex; align-items: center; justify-content: center; flex-shrink: 0; margin-right: 12px; }
.menu-emoji { font-size: 18px; }
.menu-label { flex: 1; font-size: 15px; font-weight: 500; color: #333; }
.menu-arrow { color: #ccc; font-size: 14px; }

.logout-item { border-bottom: none; }
.logout-label { color: #e74c3c; }

.pwd-field { margin-bottom: 8px; border-radius: 8px; background: #f5f7fa; }
.ver-code-img { width: 72px; height: 28px; border-radius: 6px; cursor: pointer; }
</style>
