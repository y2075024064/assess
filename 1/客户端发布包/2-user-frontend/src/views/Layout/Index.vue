<template>
  <div class="flex flex-col flex-grow overflow-hidden">
    <router-view />
  </div>
  <!-- 米奈关闭后的重新召唤按钮 -->
  <div v-if="!minaiStore.showPet" class="mina-recall" @click="minaiStore.showMinai()">
    <span class="recall-icon">🐼</span>
    <span class="recall-text">召唤米奈</span>
  </div>

  <MinaiPet />
  <van-tabbar class="flex-shrink-0" v-model="active" placeholder route safe-area-inset-bottom>
    <van-tabbar-item replace to="/activity" icon="wap-home-o">活动</van-tabbar-item>
    <van-tabbar-item replace to="/course" icon="comment">课程</van-tabbar-item>
    <van-tabbar-item replace to="/test" icon="records">练习</van-tabbar-item>
    <van-tabbar-item replace to="/email" icon="edit">信箱</van-tabbar-item>
    <van-tabbar-item replace to="/user" icon="user-o">我的</van-tabbar-item>
  </van-tabbar>
</template>

<script setup lang="ts">
import http from '@http'
import { ref } from 'vue'
import { RouterView } from 'vue-router'
import { useUserStore } from '@store/user'
import { useMinaiStore } from '@store/minai'
import MinaiPet from '@components/MinaiPet.vue'

const minaiStore = useMinaiStore()
const userStore = useUserStore()
const active = ref(1)

// 后台静默加载用户信息，不阻塞页面渲染
if (!userStore.isLogin) {
  http
    .get('api/Member/Info')
    .right((res) => {
      userStore.setUserInfo({
        name: res.Data.NickName || res.Data.Account,
        type: res.Data.MemberType,
        gender: res.Data.Gender,
        avatar: res.Data.Avatar
      })
    })
}
</script>

<style scoped>
.mina-recall {
  position: fixed;
  right: 16px;
  bottom: 70px;
  z-index: 9998;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2px;
  cursor: pointer;
  animation: recallPop 0.3s ease-out;
}

.recall-icon {
  font-size: 36px;
  animation: float 3s ease-in-out infinite;
  filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.15));
}

.recall-text {
  font-size: 11px;
  color: #666;
  background: rgba(255, 255, 255, 0.95);
  padding: 2px 10px;
  border-radius: 10px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.1);
  white-space: nowrap;
}

@keyframes recallPop {
  from { transform: scale(0); opacity: 0; }
  to { transform: scale(1); opacity: 1; }
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-6px); }
}
</style>
