<template>
  <div class="minai-page">
    <!-- 顶部导航 -->
    <van-nav-bar
      fixed
      left-arrow
      placeholder
      title="米奈 AI"
      safe-area-inset-top
      class="flex-shrink-0"
      @click-left="urlBack"
    >
      <template #right>
        <van-icon name="delete-o" size="18" @click="onClear" />
      </template>
    </van-nav-bar>

    <!-- 对话区域 -->
    <div class="chat-box" ref="chatRef">
      <div v-if="store.messages.length === 0" class="empty-state">
        <div class="empty-avatar">🐼</div>
        <div class="empty-title">你好呀！我是「米奈」✨</div>
        <div class="empty-desc">你的专属AI学习伙伴，快来和我聊天吧~</div>
      </div>

      <div v-for="(msg, i) in store.messages" :key="i" :class="['msg-row', msg.role]">
        <div v-if="msg.role === 'assistant'" class="avatar">🐼</div>
        <div :class="['bubble-wrapper', msg.role]">
          <div class="bubble">{{ msg.content }}</div>
          <div class="time">{{ msg.time }}</div>
        </div>
      </div>

      <!-- 加载中 -->
      <div v-if="store.isLoading" class="msg-row assistant">
        <div class="avatar">🐼</div>
        <div class="bubble loading-dots">
          <span></span><span></span><span></span>
        </div>
      </div>
    </div>

    <!-- 输入区域 -->
    <div class="input-area" safe-area-inset-bottom>
      <van-field
        v-model="inputText"
        placeholder="跟米奈聊聊天吧..."
        :border="false"
        @keypress.enter="onSend"
      />
      <van-button
        :disabled="!inputText.trim() || store.isLoading"
        class="send-btn"
        @click="onSend"
      >
        发送
      </van-button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, nextTick, watch, onMounted } from 'vue'
import { useMinaiStore } from '@store/minai'
import { showDialog } from 'vant'

const store = useMinaiStore()
const inputText = ref('')
const chatRef = ref<HTMLElement | null>(null)

const urlBack = () => history.back()

// 首次进入时添加欢迎消息
onMounted(() => {
  if (store.messages.length === 0) {
    store.addMessage('assistant', '你好呀！我是「米奈」✨，你的专属AI学习伙伴！有什么想问我的吗？😊')
  }
})

// 自动滚动到底部
watch(
  () => store.messages.length,
  async () => {
    await nextTick()
    if (chatRef.value) {
      chatRef.value.scrollTop = chatRef.value.scrollHeight
    }
  }
)

function onSend() {
  const text = inputText.value.trim()
  if (!text || store.isLoading) return
  store.sendMessage(text)
  inputText.value = ''
}

function onClear() {
  showDialog({
    title: '清空对话',
    message: '确定要清空所有聊天记录吗？',
    confirmButtonText: '确定'
  }).then(() => {
    store.clearMessages()
  })
}
</script>

<style scoped>
.minai-page {
  height: 100%;
  display: flex;
  flex-direction: column;
  background: #f5f5f5;
}

.chat-box {
  flex: 1;
  overflow-y: auto;
  padding: 12px 16px;
  -webkit-overflow-scrolling: touch;
}

/* 空状态 */
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding-top: 80px;
}

.empty-avatar {
  font-size: 72px;
  animation: float 3s ease-in-out infinite;
}

.empty-title {
  font-size: 18px;
  font-weight: 600;
  color: #333;
  margin-top: 16px;
}

.empty-desc {
  font-size: 14px;
  color: #999;
  margin-top: 8px;
}

/* 消息行 */
.msg-row {
  display: flex;
  margin-bottom: 16px;
  align-items: flex-start;
}

.msg-row.user {
  flex-direction: row-reverse;
}

.avatar {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
  flex-shrink: 0;
}

.bubble-wrapper {
  max-width: 70%;
  margin: 0 10px;
}

.bubble-wrapper.user {
  margin-right: 14px;
}

.bubble {
  padding: 10px 14px;
  border-radius: 14px;
  font-size: 14px;
  line-height: 1.5;
  word-break: break-word;
}

.assistant .bubble {
  background: #fff;
  color: #333;
  border-top-left-radius: 4px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);
}

.user .bubble {
  background: #1989fa;
  color: #fff;
  border-top-right-radius: 4px;
}

.time {
  font-size: 11px;
  color: #bbb;
  margin-top: 4px;
  text-align: right;
}

.user .time {
  text-align: left;
}

/* 加载动画 */
.loading-dots {
  display: flex;
  gap: 4px;
  padding: 14px 18px !important;
}

.loading-dots span {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #ccc;
  animation: bounce 1.4s ease-in-out infinite;
}

.loading-dots span:nth-child(2) { animation-delay: 0.2s; }
.loading-dots span:nth-child(3) { animation-delay: 0.4s; }

/* 输入区域 */
.input-area {
  display: flex;
  align-items: center;
  padding: 8px 12px;
  background: #fff;
  border-top: 1px solid #eee;
}

.input-area :deep(.van-field) {
  flex: 1;
  background: #f5f5f5;
  border-radius: 20px;
  padding: 4px 12px;
}

.send-btn {
  margin-left: 8px;
  border: none;
  background: #1989fa;
  color: #fff;
  border-radius: 20px;
  padding: 6px 18px;
  font-size: 14px;
  flex-shrink: 0;
}

.send-btn[disabled] {
  background: #ccc;
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

@keyframes bounce {
  0%, 80%, 100% { transform: scale(0); }
  40% { transform: scale(1); }
}
</style>
