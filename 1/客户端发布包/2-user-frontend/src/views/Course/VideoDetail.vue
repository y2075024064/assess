<template>
  <!-- 导航栏 -->
  <van-nav-bar 
    fixed 
    placeholder 
    left-arrow 
    title="视频详情页"
    safe-area-inset-top 
    class="flex-shrink-0 nav-bar"
    @click-left="onClickLeft"
  >
    <template #right>
      <div class="nav-decoration"></div>
    </template>
  </van-nav-bar>

  <!-- 内容区域 -->
  <div class="main-content">
    <!-- 视频容器 -->
    <div class="video-container">
      <video
        ref="videoPlayer"
        class="video-js vjs-big-play-centered"
        controls
        preload="auto"
      ></video>
    </div>

    <!-- 视频信息 -->
    <div class="content-card">
      <h1 class="video-title">Vue 3 零基础入门教程</h1>
      <div class="video-meta">
        <span class="meta-item">
          <van-icon name="play-circle" />1.2万次播放
        </span>
        <span class="meta-item">
          <van-icon name="clock" />2023-10-25更新
        </span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import { useRouter } from 'vue-router'
import videojs from 'video.js'
import 'video.js/dist/video-js.css'

const router = useRouter()
const videoPlayer = ref<HTMLVideoElement | null>(null)
let player: videojs.Player | null = null

// 播放器配置
const playerConfig = {
  controls: true,
  autoplay: false,
  fluid: false,
  controlBar: {
    children: [
      'playToggle',
      'volumePanel',
      'currentTimeDisplay',
      'progressControl', 
      'fullscreenToggle'
    ]
  },
  sources: [{
    src: 'https://media.w3.org/2010/05/sintel/trailer.mp4',
    type: 'video/mp4'
  }]
}

// 生命周期
onMounted(() => {
  initVideoPlayer()
  window.addEventListener('resize', updateVideoSize)
})

onBeforeUnmount(() => {
  disposeVideoPlayer()
  window.removeEventListener('resize', updateVideoSize)
})

// 初始化播放器
const initVideoPlayer = () => {
  if (videoPlayer.value) {
    player = videojs(videoPlayer.value, playerConfig)
    
    player.on('click', togglePlay)
    player.on('seeked', handleSeeked)
    
    updateVideoSize()
  }
}

// 销毁播放器
const disposeVideoPlayer = () => {
  if (player) {
    player.off('click', togglePlay)
    player.off('seeked', handleSeeked)
    player.dispose()
  }
}

// 路由返回处理
const onClickLeft = () => {
  if (window.history.state?.back) {
    router.back()
  } else {
    router.push('/course') // 替换为你的课程列表页路由
  }
}

// 播放控制
const togglePlay = () => {
  player && player.paused() ? player.play() : player.pause()
}

// 调整视频尺寸
const updateVideoSize = () => {
  const container = document.querySelector('.video-container')
  if (container?.parentElement) {
    const maxWidth = Math.min(container.parentElement.clientWidth, 800)
    container.style.width = `${maxWidth}px`
    container.style.height = `${(maxWidth * 9) / 16}px`
  }
}

// 处理进度跳转
const handleSeeked = () => {
  if (player?.paused()) {
    player.play()
  }
}
</script>

<style scoped>
.main-content {
  @apply w-full h-full flex-grow overflow-x-hidden overflow-y-auto bg-gray-50;
}

/* 导航栏样式 */
.nav-bar {
  :deep(.van-nav-bar__content) {
    background: linear-gradient(135deg, #2c3e50, #3498db);
  }

  :deep(.van-nav-bar__title) {
    @apply text-white text-[18px];
  }

  :deep(.van-icon-arrow-left) {
    @apply text-white text-[20px];
  }
}

/* 视频容器 */
.video-container {
  @apply relative mx-auto my-4 bg-black rounded-lg shadow-md;
  max-width: 800px;
  aspect-ratio: 16 / 9; /* 设置宽高比为16:9 */
}

/* 视频信息卡片 */
.content-card {
  @apply bg-white mx-4 my-4 p-4 rounded-lg shadow-sm;
}

.video-title {
  @apply text-gray-800 text-xl mb-2;
}

.video-meta {
  @apply text-gray-600 text-sm;
}

/* 覆盖video.js样式 */
:deep(.video-js) {
  width: 100% !important;
  height: 100% !important;

}
</style>