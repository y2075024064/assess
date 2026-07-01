<template>
  <van-nav-bar fixed placeholder safe-area-inset-top class="custom-navbar flex-shrink-0">
    <template #title>
      <div class="nav-title"><span>✉️</span> 计算思维信箱 <span class="cq-tag">重庆</span></div>
    </template>
  </van-nav-bar>

  <!-- 搜索栏 -->
  <div class="search-wrapper">
    <van-search v-model="keyword" placeholder="🔍 请输入搜索关键词" @search="onSearch" class="custom-search" shape="round" />
    <van-button class="publish-btn" color="linear-gradient(135deg, #667eea, #764ba2)" @click="toAddLetter" size="small" round>
      + 发布
    </van-button>
  </div>

  <!-- 信件列表 -->
  <van-pull-refresh class="letter-list-wrap" v-model="refreshing" @refresh="onRefresh">
    <van-list v-model:loading="loading" finished-text="— 没有更多了 —" :finished="finished" @load="onLoad">
      <div class="letter-card" v-for="(item, index) in problemList" :key="index" @click="gotoLetterDetail(item.Id)">
        <div class="letter-head">
          <van-image class="letter-avatar" height="44" width="44" fit="cover" round :src="item.Avatar" />
          <div class="letter-user">
            <div class="letter-title">{{ item.Title }}</div>
            <div class="letter-name">{{ item.Nickname }}</div>
          </div>
          <van-tag :type="item.IsModel ? 'danger' : 'warning'" plain round class="letter-tag">
            {{ item.IsModel ? '精华' : states[item.State] }}
          </van-tag>
        </div>

        <div class="letter-body">
          <div class="letter-content">{{ item.Content }}</div>
          <van-image v-if="item.Image" class="letter-image" width="6rem" height="5rem" fit="cover" radius="8" :src="item.Image" />
        </div>

        <div class="letter-tags" v-if="item.Label">
          <van-tag v-for="(label, LIndex) in JSON.parse(item.Label)" :key="LIndex" round color="#f0e6ff" text-color="#764ba2" class="tag-item">
            {{ label }}
          </van-tag>
        </div>

        <div class="letter-footer">
          <span>👁️ {{ item.ViewNum }}</span>
          <span>💬 {{ item.CommentNum }}</span>
          <span>📅 {{ item.CreationTime }}</span>
        </div>
      </div>
    </van-list>
  </van-pull-refresh>
</template>

<script setup lang="ts">
import http from '@http'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import type { LetterInfo } from '@/types/LetterInfo'

const router = useRouter()

const keyword = ref('')
const orderType = ref(0)
const loading = ref(false)
const finished = ref(false)
const refreshing = ref(false)
const problemList = ref<LetterInfo[]>([])

let page = 1
const take = 5
const states: any = {
  New: '新增',
  Viewed: '已查看',
  Processing: '处理中',
  Resolved: '已解决',
  Finished: '已完成'
}

/** 信件搜索 */
const onSearch = () => {
  onRefresh()
}

/** 下拉刷新 */
const onRefresh = () => {
  // 清空列表数据
  page = 1
  problemList.value = []
  // 重新加载数据
  finished.value = false
}

/** 分页获取信件列表 */
const onLoad = () => {
  const param = {
    OrderType: orderType.value,
    Keyword: keyword.value,
    Page: page,
    Take: take
  }
  loading.value = true
  http
    .post('api/Letter/PublicList', param)
    .right((res) => {
      const arr = res.Data as LetterInfo[]
      if (arr.length) {
        arr.forEach((e) => problemList.value.push(e))
        ++page
      } else {
        finished.value = true
      }
    })
    .finally(() => {
      loading.value = false
      if (refreshing.value) refreshing.value = false
    })
}

/** 前往信件详情页面 */
const gotoLetterDetail = (id: number) => {
  router.push({ name: 'letterDetail', query: { id } })
}

/** 用户发布信件 */
const toAddLetter = () => {
  router.push({ name: 'addLetter' })
}
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

.search-wrapper {
  display: flex; align-items: center;
  background: #fff; padding: 4px 12px 4px 4px;
  border-bottom: 1px solid #f0f0f0;
}

.custom-search {
  flex: 1;
  --van-search-background: #f5f7fa;
}

.publish-btn {
  flex-shrink: 0; margin-left: 8px; font-weight: 600;
}

.letter-list-wrap {
  height: 100%; overflow-y: auto;
  background: linear-gradient(180deg, #f8f9ff 0%, #f0f2f5 100%);
}

.letter-card {
  background: #fff; margin: 10px 12px; padding: 14px 16px;
  border-radius: 16px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.04);
  transition: all 0.2s; cursor: pointer;
  &:active { transform: scale(0.98); background: #fafbff; }
}

.letter-head {
  display: flex; align-items: center; gap: 12px; margin-bottom: 10px;
}

.letter-avatar {
  flex-shrink: 0; border: 2px solid #f0e6ff;
}

.letter-user { flex: 1; min-width: 0; }

.letter-title {
  @apply text-base font-bold; color: #333;
  overflow: hidden; text-overflow: ellipsis; white-space: nowrap;
}

.letter-name { @apply text-xs; color: #999; margin-top: 2px; }

.letter-tag { flex-shrink: 0; font-size: 11px; padding: 2px 8px; }

.letter-body {
  display: flex; gap: 10px;
}

.letter-content {
  flex: 1; @apply text-sm leading-relaxed; color: #555;
  overflow: hidden;
  display: -webkit-box; -webkit-line-clamp: 3; -webkit-box-orient: vertical;
}

.letter-image { flex-shrink: 0; border-radius: 8px; }

.letter-tags {
  display: flex; flex-wrap: wrap; gap: 6px; margin-top: 10px;
}

.tag-item { font-size: 12px; padding: 2px 10px; }

.letter-footer {
  display: flex; gap: 16px; margin-top: 12px; padding-top: 10px;
  border-top: 1px solid #f5f5f5;
  @apply text-xs; color: #bbb;
}

.ellipsis {
  display: -webkit-box; -webkit-line-clamp: 3;
  -webkit-box-orient: vertical; overflow: hidden; text-overflow: ellipsis;
}
</style>
