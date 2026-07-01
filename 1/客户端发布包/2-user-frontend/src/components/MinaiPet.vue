<template>
  <div
    v-if="store.showPet"
    class="minai-pet"
    :style="{ left: store.posX + 'px', top: store.posY + 'px' }"
    @mousedown="onDragStart"
    @touchstart.prevent="onTouchStart"
  >
    <!-- 关闭按钮 -->
    <div class="close-btn" @click.stop="store.hidePet()">✕</div>

    <!-- 米奈形象 -->
    <div class="pet-body" @click.stop="onPetClick">
      <div class="pet-avatar">🐼</div>
      <div class="pet-name">米奈</div>
    </div>

    <!-- 消息气泡 -->
    <div v-if="lastMessage" class="speech-bubble">
      {{ lastMessage }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useMinaiStore } from '@store/minai'

const store = useMinaiStore()
const router = useRouter()

// 米奈随机语录（点击切换，3秒消失）
const phrases = [
  '来和我聊天吧~ 🐼',
  '等你很久啦~ ✨',
  '今天想学点什么？📚',
  '嘿，我在这里！👋',
  '一起加油学习吧！💪',
  '戳我干嘛~ 😝',
  '嘿嘿~ 你好呀！🌸',
  '我在呢我在呢~ 🙋',
  '要不要聊聊天？💬',
  '今天心情好好呀~ 🌈',
  '认真学习最棒啦！📖',
  '你找我呀？👀',
  '嘿嘿嘿~ 😊',
  '加油加油！⚡',
  '米奈在此！🐼✨'
]
const bubbleText = ref('')
let bubbleTimer = 0

/** 显示随机语录，3秒后消失 */
function showBubble() {
  clearTimeout(bubbleTimer)
  const pool = phrases.filter(p => p !== bubbleText.value)
  bubbleText.value = pool[Math.floor(Math.random() * pool.length)]
  bubbleTimer = window.setTimeout(() => { bubbleText.value = '' }, 3000)
}

// 首次出现显示随机语录
onMounted(() => showBubble())

/** 点击米奈：换句话 + 跳转到聊天页 */
function onPetClick() {
  showBubble()
  router.push('/minai')
}

const lastMessage = computed(() => {
  if (bubbleText.value) return bubbleText.value
  const msgs = store.messages
  if (msgs.length === 0) return null
  const last = msgs[msgs.length - 1]
  return last.role === 'assistant' ? last.content.slice(0, 20) + (last.content.length > 20 ? '…' : '') : null
})

let isDragging = false
let startX = 0
let startY = 0
let originX = 0
let originY = 0

function onDragStart(e: MouseEvent) {
  isDragging = true
  startX = e.clientX
  startY = e.clientY
  originX = store.posX
  originY = store.posY
  document.addEventListener('mousemove', onDragMove)
  document.addEventListener('mouseup', onDragEnd)
}

function onDragMove(e: MouseEvent) {
  if (!isDragging) return
  const dx = e.clientX - startX
  const dy = e.clientY - startY
  store.setPosition(originX + dx, originY + dy)
}

function onDragEnd() {
  isDragging = false
  document.removeEventListener('mousemove', onDragMove)
  document.removeEventListener('mouseup', onDragEnd)
}

function onTouchStart(e: TouchEvent) {
  const touch = e.touches[0]
  isDragging = true
  startX = touch.clientX
  startY = touch.clientY
  originX = store.posX
  originY = store.posY
  document.addEventListener('touchmove', onTouchMove)
  document.addEventListener('touchend', onTouchEnd)
}

function onTouchMove(e: TouchEvent) {
  if (!isDragging) return
  const touch = e.touches[0]
  const dx = touch.clientX - startX
  const dy = touch.clientY - startY
  store.setPosition(originX + dx, originY + dy)
}

function onTouchEnd() {
  isDragging = false
  document.removeEventListener('touchmove', onTouchMove)
  document.removeEventListener('touchend', onTouchEnd)
}
</script>

<style scoped>
.minai-pet {
  position: fixed;
  z-index: 9999;
  cursor: grab;
  user-select: none;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.minai-pet:active {
  cursor: grabbing;
}

.close-btn {
  position: absolute;
  top: -8px;
  right: -8px;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: rgba(0, 0, 0, 0.5);
  color: #fff;
  font-size: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  opacity: 0;
  transition: opacity 0.2s;
}

.minai-pet:hover .close-btn {
  opacity: 1;
}

.pet-body {
  display: flex;
  flex-direction: column;
  align-items: center;
  animation: float 3s ease-in-out infinite;
}

.pet-avatar {
  font-size: 48px;
  line-height: 1;
  filter: drop-shadow(0 4px 8px rgba(0, 0, 0, 0.15));
  transition: transform 0.2s;
}

.pet-body:hover .pet-avatar {
  transform: scale(1.1);
}

.pet-name {
  font-size: 11px;
  color: #666;
  margin-top: 2px;
  background: rgba(255, 255, 255, 0.9);
  padding: 1px 8px;
  border-radius: 8px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.1);
}

.speech-bubble {
  position: absolute;
  right: 60px;
  top: 0;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 12px;
  padding: 6px 12px;
  font-size: 12px;
  color: #333;
  max-width: 140px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.speech-bubble::after {
  content: '';
  position: absolute;
  right: -6px;
  top: 12px;
  border-left: 6px solid #fff;
  border-top: 4px solid transparent;
  border-bottom: 4px solid transparent;
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-8px); }
}
</style>
