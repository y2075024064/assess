<template>
  <!-- 顶部导航 -->
  <van-nav-bar class="custom-navbar flex-shrink-0" fixed placeholder safe-area-inset-top>
    <template #title>
      <div class="nav-title">
        <span>🧮</span> 计算思维练习 <span class="cq-tag">重庆</span>
      </div>
    </template>
  </van-nav-bar>

  <!-- 年级选择器 -->
  <div class="grade-wrapper">
    <div class="grade-scroll" id="gradeList">
      <div class="grade-track">
        <div class="grade-filler"></div>
        <div
          v-for="(item, index) in tabbarList"
          :key="index"
          class="grade-item"
          :class="{ active: gradeActive === index }"
          @click="onGradeScroll(index)"
        >
          <span class="grade-icon">{{ gradeIcons[index % gradeIcons.length] }}</span>
          <span class="grade-text">{{ item }}</span>
        </div>
        <div class="grade-filler"></div>
      </div>
    </div>
  </div>

  <!-- 主功能区 -->
  <div class="main-content">
    <!-- 两个核心入口 -->
    <div class="core-section">
      <div class="core-card order-card" @click="gotoPage('order')">
        <div class="core-circle">
          <div class="core-number">{{ testedNumber }}<small>/{{ totalNumber }}</small></div>
          <div class="core-label">已练习</div>
        </div>
        <div class="core-info">
          <div class="core-title">📝 顺序练习</div>
          <div class="core-desc">按顺序逐题练习，巩固知识</div>
        </div>
      </div>

      <div class="core-card test-card" @click="gotoPage('test')">
        <div class="core-circle">
          <div class="core-score">{{ topScore }}</div>
          <div class="core-label">最高分</div>
        </div>
        <div class="core-info">
          <div class="core-title">🏆 模拟测评</div>
          <div class="core-desc">模拟真实考试，挑战自我</div>
        </div>
      </div>
    </div>

    <!-- 功能入口网格 -->
    <div class="func-grid">
      <div class="func-item" v-for="item in funcList" :key="item.key" @click="gotoPage(item.key)">
        <div class="func-icon" :style="{ background: item.bg }">
          <span class="func-emoji">{{ item.emoji }}</span>
        </div>
        <span class="func-name">{{ item.name }}</span>
      </div>
    </div>

    <!-- 计算思维之星 -->
    <div class="star-section">
      <div class="star-header">
        <span class="star-title-icon">⭐</span>
        <span class="star-title">计算思维之星</span>
        <span class="star-line"></span>
      </div>
      <van-swipe :autoplay="10000" lazy-render :show-indicators="false" class="star-swipe">
        <van-swipe-item v-for="(item, index) in thinkStarList" :key="index" class="star-slide">
          <div
            v-for="(subItem, subIndex) in item"
            :key="subIndex"
            class="star-card"
            @click="goToThinkStarDetail(subItem.Id)"
          >
            <van-image height="100%" width="100%" fit="cover" radius="12" :src="subItem.PreviewImg" position="center" />
            <div class="star-card-mask">
              <div class="star-card-type">{{ subItem.Type }}</div>
              <div class="star-card-title">{{ subItem.Title }}</div>
            </div>
          </div>
        </van-swipe-item>
      </van-swipe>
    </div>
  </div>

  <!-- 专项练习 - 标签搜索引擎弹窗 -->
  <van-popup v-model:show="showPartPopup" position="right" :style="{ width: '100%', height: '100%' }">
    <div class="flex flex-col h-full bg-white">
      <!-- 顶部：搜索栏 -->
      <div class="flex-shrink-0 px-4 pt-12 pb-4" style="background: linear-gradient(135deg, #667eea, #764ba2);">
        <div class="flex items-center gap-2">
          <van-icon name="arrow-left" color="#fff" size="20" @click="showPartPopup = false" />
          <div class="flex-1 flex items-center bg-white rounded-full px-4 py-2.5 shadow-sm">
            <van-icon name="search" color="#667eea" size="16" />
            <input
              v-model="searchKeyword"
              class="flex-1 border-none outline-none text-sm ml-2"
              placeholder="输入标签搜索题目..."
              @keyup.enter="onSearchTopic"
            />
            <span v-if="searchKeyword" class="text-gray-400 text-sm cursor-pointer" @click="onClearSearch">✕</span>
          </div>
          <span class="text-white text-sm font-medium px-2" @click="onSearchTopic">搜索</span>
        </div>
        <!-- 提示 -->
        <div class="text-xs text-white/70 mt-2 ml-1">💡 输入标签关键词搜索，如：算法、编程、二进制</div>
      </div>

      <!-- 所属专项（网格布局，一排两个） -->
      <div v-if="!searched" class="px-4 pt-4 pb-2 border-b border-gray-50">
        <div class="grid grid-cols-2 gap-3">
          <div
            v-for="(item, idx) in partList"
            :key="idx"
            class="flex flex-col items-center justify-center px-3 py-4 bg-gradient-to-br from-indigo-50 to-purple-50 rounded-xl cursor-pointer active:opacity-80 min-h-[90px]"
            @click="onPartClick(item)"
          >
            <span class="text-2xl mb-1.5">{{ ['🎯','🧩','🎨','🔬','📐','💻','🌐','🧠'][idx % 8] }}</span>
            <div class="text-sm font-semibold text-gray-800 text-center">{{ item.Name }}</div>
            <div class="text-[10px] text-gray-400 mt-0.5">{{ item.TopicCount }}道题目</div>
          </div>
          <div v-if="partList.length === 0" class="col-span-2 text-center text-gray-300 text-sm py-4">暂无专项</div>
        </div>
      </div>
    </div>
  </van-popup>
</template>

<script setup lang="ts">
import partTest from '@assets/images/partTest.png'
import randomTest from '@assets/images/randomTest.png'
import scoreRank from '@assets/images/scoreRank.png'
import myScore from '@assets/images/myScore.png'
import collectTopics from '@assets/images/collectTopics.png'
import errorTopics from '@assets/images/errorTopics.png'
import { ref, onMounted, onBeforeMount } from 'vue'
import { getItem, setItem } from '@/utils/storage'
import { dragMoveX } from '@/utils/dragMove'
import { useAppStore } from '@store/app'
import { useRouter, useRoute } from 'vue-router'
import { showNotify } from 'vant'
import http from '@http'

const router = useRouter()
const route = useRoute()

const gradeIcons = ['🌸', '🌻', '🍀', '🌺', '🌷', '🌼', '🌿', '🌱', '🪷']

const funcList = [
  { key: 'part', name: '专项练习', emoji: '🎯', bg: 'linear-gradient(135deg, #667eea, #764ba2)' },
  { key: 'random', name: '随机练习', emoji: '🎲', bg: 'linear-gradient(135deg, #f093fb, #f5576c)' },
  { key: 'scoreList', name: '成绩榜单', emoji: '🏅', bg: 'linear-gradient(135deg, #4facfe, #00f2fe)' },
  { key: 'scoreRecord', name: '我的成绩', emoji: '📊', bg: 'linear-gradient(135deg, #43e97b, #38f9d7)' },
  { key: 'collectTopic', name: '我的收藏', emoji: '💎', bg: 'linear-gradient(135deg, #fa709a, #fee140)' },
  { key: 'errorTopic', name: '我的错题', emoji: '📝', bg: 'linear-gradient(135deg, #a18cd1, #fbc2eb)' },
]

const showPartPopup = ref(false)
const topScore = ref(0)
const totalNumber = ref(0)
const gradeActive = ref(0)
const tabbarList = ref([])
const testedNumber = ref(0)
const typeFieldText = ref('')
const typeFieldValue = ref(0)
const showTypePicker = ref(false)
const typeColumns = ref([{ Text: '', Value: 0 }])
const partColumns = ref([{ Text: '', Value: 0 }])

const thinkStarList = ref<thinkStarInfo[][]>([])
const active = ref('0')

// 搜索引擎相关
const searchKeyword = ref('')
const searched = ref(false)
const selectedPartId = ref<number | null>(null)
const partList = ref<any[]>([])
let searchTimer: number | undefined

type thinkStarInfo = { PreviewImg: string; Title: number; Type: string; Id: number }

const getGradeName = () => {
  let grade = ''
  const index = gradeActive.value
  const len = tabbarList.value.length
  if (len > 0 && index < len) grade = tabbarList.value[index]
  return grade
}

const getTypePartId = () => {
  const _partId = typeFieldValue.value
  if (_partId > 0 && typeColumns.value.length > 0) {
    if (typeColumns.value.findIndex((e) => e.Value == _partId) >= 0) return _partId
  }
  typeFieldText.value = ''
  typeFieldValue.value = 0
  return null
}

const saveUserSelection = () => {
  const data = `${gradeActive.value}_${typeFieldValue.value}_${typeFieldText.value}`
  setItem('test-page-selection', data)
}

const loadUserSelection = () => {
  const data = getItem('test-page-selection')
  if (data) {
    const datas = data.split('_')
    typeFieldText.value = decodeURI(datas[2])
    gradeActive.value = Number.parseInt(datas[0])
    typeFieldValue.value = Number.parseInt(datas[1])
  }
}

const getTypeList = () => {
  const grade = getGradeName()
  http.get('api/TopicPart/List', { params: { grade } }).right((res) => {
    typeColumns.value = res.Data
    if (typeColumns.value.length) {
      const first = typeColumns.value[0]
      typeFieldValue.value = first.Value
      typeFieldText.value = first.Text
    }
  }).finally(() => { getTotalNumber(); getCurrNumber(); getMaxScore() })
}

const getTotalNumber = () => {
  const grade = getGradeName(); const partId = getTypePartId()
  http.get('api/Topic/TotalNumber', { params: { grade, partId } }).right((res) => { totalNumber.value = res.Data })
}

const getCurrNumber = () => {
  const grade = getGradeName(); const partId = getTypePartId()
  http.get('api/Practice/CurrNumber', { params: { grade, partId } }).right((res) => { testedNumber.value = res.Data })
}

const getMaxScore = () => {
  http.get('api/MockTest/MaxScore?partId=').right((res) => { topScore.value = res.Data })
}

const selectPart = () => {
  // 直接打开搜索引擎（不需要先选类别）
  searchKeyword.value = ''
  selectedPartId.value = null
  showPartPopup.value = true
  loadPartList()
}

/** 加载所属专项列表 */
const loadPartList = () => {
  const grade = getGradeName()
  http.get('api/TopicPart/List', { params: { grade } }).right((res) => {
    const parts = res.Data as Array<any>
    partList.value = (parts || []).map((p: any) => ({ Id: p.Value, Name: p.Text, TopicCount: '...' }))
    ;(parts || []).forEach((p: any) => {
      http.get('api/Topic/TotalNumber', { params: { grade, partId: p.Value } }).right((r) => {
        const idx = partList.value.findIndex((x: any) => x.Id === p.Value)
        if (idx >= 0) partList.value[idx] = { ...partList.value[idx], TopicCount: r.Data }
      })
    })
  })
}

/** 点击专项进入练习 */
const onPartClick = (item: any) => {
  showPartPopup.value = false
  const grade = getGradeName()
  router.push({ name: 'practice', query: { grade, partId: item.Id, type: 'part' } })
}

/** 执行搜索 - 直接进入标签练习 */
const onSearchTopic = () => {
  const kw = searchKeyword.value.trim()
  if (!kw) return
  showPartPopup.value = false
  router.push({ name: 'practice', query: { tag: kw, type: 'tag' } })
}

/** 清除搜索 */
const onClearSearch = () => {
  searchKeyword.value = ''
}



const onGradeScroll = (index: number) => {
  gradeActive.value = index
  const ele = document.getElementById('gradeList') as HTMLElement
  ele.scrollLeft = 100 * index
  saveUserSelection(); getTypeList()
}

const onTypeConfirm = (event: any) => {
  const item = event.selectedOptions[0]
  showTypePicker.value = false
  typeFieldText.value = item.Text; typeFieldValue.value = item.Value
  saveUserSelection(); getTotalNumber(); getCurrNumber(); getMaxScore()
}

const gotoPage = (type: string) => {
  let query = {}; let name = 'practice'
  switch (type) {
    case 'random': case 'error': case 'order': case 'test':
      query = { grade: getGradeName(), partId: typeFieldValue.value, type }; break
    case 'part': selectPart(); return
    default: name = type; break
  }
  router.push({ name, query })
}

const goToThinkStarDetail = (id: number) => { router.push({ name: 'thinkStar', query: { id } }) }

const getGradeList = () => {
  http.get('api/Grade/List').right((res) => {
    tabbarList.value = res.Data
    setTimeout(() => onGradeScroll(gradeActive.value), 500)
  }).finally(() => { getTypeList() })
}

const getThinkStarList = () => {
  http.get('api/ThinkStar/List', { params: { num: 12 } }).right((res) => {
    for (let i = 0; i < res.Data.length; i += 4)
      thinkStarList.value.push(res.Data.slice(i, i + 4))
  })
}

onBeforeMount(() => {
  loadUserSelection(); getThinkStarList(); getGradeList()
  // 从标签练习返回时立即打开搜索引擎（先于页面渲染）
  if (route.query.openTagSearch === '1') {
    searchKeyword.value = ''
    selectedPartId.value = null
    showPartPopup.value = true
    loadPartList()
  }
})
onMounted(() => {
  const { isMobile } = useAppStore()
  dragMoveX(document.getElementById('test-tabbar-box'), isMobile)
})
</script>

<style lang="scss" scoped>
/* ===== 导航栏 ===== */
.custom-navbar {
  --van-nav-bar-background: linear-gradient(135deg, #667eea, #764ba2);
  --van-nav-bar-title-text-color: #fff;

  :deep(.van-nav-bar__title) {
    @apply text-lg font-bold;
  }
}

.nav-title {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
}

.cq-tag {
  @apply text-xs px-1.5 py-0.5 rounded-full ml-1;
  background: rgba(255,255,255,0.25);
  color: #fff;
  font-weight: 400;
}

/* ===== 年级选择 ===== */
.grade-wrapper {
  @apply bg-white px-2 py-2;
  border-bottom: 1px solid #f0f0f0;
}

.grade-scroll {
  width: 100%;
  overflow-x: auto;
  overflow-y: hidden;
  -webkit-overflow-scrolling: touch;

  &::-webkit-scrollbar { display: none; }
}

.grade-track {
  display: flex;
  gap: 8px;
  padding: 2px 0;
}

.grade-filler {
  min-width: 4px;
}

.grade-item {
  @apply flex flex-col items-center justify-center px-4 py-2 rounded-2xl cursor-pointer transition-all duration-300 flex-shrink-0;
  min-width: 70px;
  background: #f5f7fa;
  border: 2px solid transparent;

  &.active {
    background: linear-gradient(135deg, #667eea, #764ba2);
    border-color: #667eea;
    box-shadow: 0 4px 15px rgba(102, 126, 234, 0.35);
    transform: translateY(-2px);

    .grade-text { color: #fff; font-weight: 700; }
    .grade-icon { font-size: 18px; }
  }

  .grade-icon { font-size: 16px; transition: all 0.3s; }
  .grade-text { @apply text-sm mt-0.5; color: #666; transition: all 0.3s; white-space: nowrap; }
}

/* ===== 主内容 ===== */
.main-content {
  @apply flex-1 overflow-y-auto px-4 pt-4 pb-2;
  background: linear-gradient(180deg, #f8f9ff 0%, #f0f2f5 100%);
}

/* ===== 核心入口 ===== */
.core-section {
  @apply flex gap-3 mb-4;
}

.core-card {
  @apply flex-1 flex items-center p-4 rounded-2xl cursor-pointer relative overflow-hidden;
  box-shadow: 0 4px 20px rgba(0,0,0,0.06);
  transition: all 0.3s;

  &:active { transform: scale(0.97); }

  &::before {
    content: '';
    position: absolute;
    top: -50%;
    right: -30%;
    width: 100px;
    height: 100px;
    border-radius: 50%;
    background: rgba(255,255,255,0.1);
  }
}

.order-card {
  background: linear-gradient(135deg, #667eea, #764ba2);
}

.test-card {
  background: linear-gradient(135deg, #f093fb, #f5576c);
}

.core-circle {
  @apply w-[70px] h-[70px] rounded-full flex flex-col items-center justify-center flex-shrink-0 mr-3;
  background: rgba(255,255,255,0.2);
  backdrop-filter: blur(4px);
}

.core-number {
  @apply text-xl font-bold leading-5;
  color: #fff;

  small { @apply text-xs font-normal; opacity: 0.8; }
}

.core-score {
  @apply text-2xl font-bold leading-6;
  color: #fff;
}

.core-label {
  @apply text-xs mt-0.5;
  color: rgba(255,255,255,0.8);
}

.core-info {
  @apply flex flex-col;
}

.core-title {
  @apply text-base font-bold;
  color: #fff;
}

.core-desc {
  @apply text-xs mt-0.5;
  color: rgba(255,255,255,0.75);
}

/* ===== 功能入口网格 ===== */
.func-grid {
  @apply grid grid-cols-3 gap-3 mb-4;
}

.func-item {
  @apply flex flex-col items-center cursor-pointer;
  transition: all 0.3s;

  &:active { transform: scale(0.92); }
}

.func-icon {
  @apply w-[52px] h-[52px] rounded-2xl flex items-center justify-center mb-1.5;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  transition: all 0.3s;

  .func-emoji { font-size: 22px; }
}

.func-name {
  @apply text-xs font-medium;
  color: #555;
}

/* ===== 思维之星 ===== */
.star-section {
  @apply mt-2;
}

.star-header {
  @apply flex items-center gap-1.5 mb-3;

  .star-title-icon { font-size: 16px; }
  .star-title { @apply text-sm font-bold; color: #444; }
  .star-line { @apply flex-1 h-px ml-2; background: linear-gradient(to right, #ddd, transparent); }
}

.star-swipe {
  @apply w-full overflow-visible;
}

.star-slide {
  @apply grid grid-cols-4 gap-2;
}

.star-card {
  @apply relative rounded-xl overflow-hidden cursor-pointer aspect-[3/4];
  box-shadow: 0 2px 10px rgba(0,0,0,0.08);
  transition: all 0.3s;

  &:active { transform: scale(0.95); }
}

.star-card-mask {
  @apply absolute bottom-0 left-0 right-0 p-1.5;
  background: linear-gradient(transparent, rgba(0,0,0,0.7));
}

.star-card-type {
  @apply text-xs;
  color: rgba(255,255,255,0.8);
}

.star-card-title {
  @apply text-xs font-bold truncate;
  color: #fff;
}
</style>
