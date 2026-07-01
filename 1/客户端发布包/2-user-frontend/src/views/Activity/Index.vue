<template>
  <van-nav-bar fixed placeholder safe-area-inset-top class="custom-navbar flex-shrink-0">
    <template #title>
      <div class="nav-title"><span>🎪</span> 计算思维活动 <span class="cq-tag">重庆</span></div>
    </template>
  </van-nav-bar>

  <div class="h-full flex flex-col flex-grow overflow-hidden act-bg">
    <!-- 轮播区 -->
    <div class="flex-shrink-0 px-3 pt-3">
      <div class="swipe-wrapper">
        <van-swipe :autoplay="3000" :height="160" :show-indicators="true" indicator-color="#667eea">
          <van-swipe-item v-for="(item, index) in AdList" :key="index" class="relative">
            <van-image fit="cover" class="w-full h-full" :src="item.PreviewImg" @click="toAdDetail(index)" />
            <div class="swipe-title">{{ item.Title }}</div>
          </van-swipe-item>
        </van-swipe>
      </div>
    </div>

    <div class="flex-1 py-3 px-3 overflow-hidden">
      <div class="h-full overflow-y-auto content-card">
        <!-- 新闻 -->
        <div class="mb-4">
          <div class="section-header">
            <div class="section-left">
              <span class="section-icon">📰</span>
              <span class="section-title">新闻动态</span>
            </div>
            <span class="section-more" @click="toMore('新闻')">
              更多 <van-icon name="arrow" />
            </span>
          </div>
          <div class="news-list">
            <div v-for="(item, index) in newsList" :key="index" class="news-item" @click="toDynamicDetail(item.Id)">
              <div class="news-dot"></div>
              <div class="news-content">
                <div class="news-title">{{ item.Title }}</div>
                <div class="news-meta">
                  <span class="news-author">{{ item.NickName }}</span>
                  <span class="news-time">{{ item.PublishTime?.substring(2, 10) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- 通知 -->
        <div>
          <div class="section-header">
            <div class="section-left">
              <span class="section-icon">🔔</span>
              <span class="section-title">通知动态</span>
            </div>
            <span class="section-more" @click="toMore('系统')">
              更多 <van-icon name="arrow" />
            </span>
          </div>
          <div class="notice-list">
            <div v-for="(item, index) in systemList" :key="index" class="notice-item" @click="toDynamicDetail(item.Id)">
              <van-image class="notice-img" :src="item.PreviewImg" radius="10" height="5.5rem" width="8rem" fit="cover" />
              <div class="notice-right">
                <div class="notice-title">{{ item.Title }}</div>
                <div class="notice-tag">通知</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { useRouter } from 'vue-router'
import { ref, onBeforeMount } from 'vue'
import { DynamicInfo } from '@/types/DynamicInfo'

type AdInfo = { Id: 0; Hyperlink: ''; PreviewImg: ''; Title: ''; Type: '' }

const router = useRouter()

const AdList = ref<AdInfo[]>([]) //当前广告列表
const newsList = ref<DynamicInfo[]>([]) //当前新闻列表
const systemList = ref<DynamicInfo[]>([]) //当前通知列表

/** 获取新闻 */
const getNewsList = () => {
  const params = { page: 1, take: 3, type: '新闻' }
  http.get('api/Dynamic/GetDynamicList', { params }).right((res) => {
    newsList.value = res.Data
  })
}

/** 获取系统通知 */
const getSystemList = () => {
  const params = { page: 1, take: 5, type: '系统' }
  http.get('api/Dynamic/GetDynamicList', { params }).right((res) => {
    systemList.value = res.Data
  })
}

/** 获取广告 */
const getAdList = () => {
  http.get('api/AdModule/GetAdInfo', { params: { moduleName: 'app_home_top' } }).right((res) => {
    AdList.value = res.Data.AdList
  })
}

/** 跳转广告详细页面 */
const toAdDetail = (index: number) => {
  if (!AdList.value[index].Hyperlink)
    router.push({ name: 'adContent', query: { id: AdList.value[index].Id } })
  else window.open(AdList.value[index].Hyperlink)
}

/** 前往动态页面 */
const toDynamicDetail = (id: number) => {
  router.push({ name: 'dynamicDetail', query: { id } })
}

/** 前往更多页面 */
const toMore = (type: string) => {
  router.push({ name: 'dynamicList', query: { Type: type } })
}

onBeforeMount(() => {
  getAdList()
  getNewsList()
  getSystemList()
})
</script>

<style lang="scss" scoped>
.custom-navbar {
  --van-nav-bar-background: linear-gradient(135deg, #667eea, #764ba2);
  --van-nav-bar-title-text-color: #fff;
  :deep(.van-nav-bar__title) { @apply text-lg font-bold; }
}

.nav-title {
  display: flex; align-items: center; justify-content: center; gap: 6px;
}

.cq-tag {
  @apply text-xs px-1.5 py-0.5 rounded-full ml-1;
  background: rgba(255,255,255,0.25); color: #fff; font-weight: 400;
}

.act-bg {
  background: linear-gradient(180deg, #f8f9ff 0%, #f0f2f5 100%);
}

.swipe-wrapper {
  border-radius: 16px; overflow: hidden;
  box-shadow: 0 4px 20px rgba(0,0,0,0.08);
}

.swipe-title {
  position: absolute; width: 100%; bottom: 0;
  padding: 8px 12px; font-size: 14px; color: #fff;
  letter-spacing: 1px;
  background: linear-gradient(transparent, rgba(0,0,0,0.6));
}

.content-card {
  background: rgba(255,255,255,0.92);
  border-radius: 16px; padding: 16px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
}

.section-header {
  display: flex; justify-content: space-between; align-items: center;
  margin-bottom: 12px;
}

.section-left {
  display: flex; align-items: center; gap: 6px;
}

.section-icon { font-size: 18px; }

.section-title {
  @apply text-base font-bold; color: #333;
}

.section-more {
  @apply text-xs; color: #999; cursor: pointer;
  &:hover { color: #667eea; }
}

.news-list {
  display: flex; flex-direction: column; gap: 0;
}

.news-item {
  display: flex; align-items: flex-start; gap: 10px;
  padding: 12px 0; cursor: pointer;
  border-bottom: 1px solid #f5f5f5;
  transition: all 0.2s;
  &:active { background: #f8f9ff; margin: 0 -8px; padding: 12px 8px; border-radius: 12px; }
  &:last-child { border-bottom: none; }
}

.news-dot {
  width: 8px; height: 8px; border-radius: 50%;
  background: linear-gradient(135deg, #667eea, #764ba2);
  margin-top: 6px; flex-shrink: 0;
}

.news-content { flex: 1; min-width: 0; }

.news-title {
  @apply text-sm font-medium; color: #333;
  overflow: hidden; text-overflow: ellipsis; white-space: nowrap;
}

.news-meta {
  display: flex; gap: 12px; margin-top: 4px;
}

.news-author { @apply text-xs; color: #999; }
.news-time { @apply text-xs; color: #bbb; }

.notice-list { display: flex; flex-direction: column; gap: 12px; }

.notice-item {
  display: flex; gap: 12px; cursor: pointer; padding: 4px;
  border-radius: 12px; transition: all 0.2s;
  &:active { background: #f8f9ff; }
}

.notice-img {
  flex-shrink: 0; border-radius: 10px;
  border: 1px solid #f0f0f0;
}

.notice-right {
  flex: 1; display: flex; flex-direction: column; justify-content: center; min-width: 0;
}

.notice-title {
  @apply text-sm font-medium; color: #333;
  overflow: hidden; text-overflow: ellipsis;
  display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical;
}

.notice-tag {
  @apply text-xs mt-2 px-2 py-0.5 inline-block rounded-full;
  background: #f0e6ff; color: #764ba2; width: fit-content;
}

.ellip {
  overflow: hidden; display: -webkit-box; -webkit-line-clamp: 4;
  text-overflow: ellipsis; -webkit-box-orient: vertical;
}

::-webkit-scrollbar { display: none; }
.van-image { flex-shrink: 0; }

::v-deep(.van-cell__title) { width: 100%; }
::v-deep(.van-cell__value) { flex-shrink: 0; width: 60px; flex: none; }
</style>
