<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="成绩榜单"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
  />

  <!-- 榜单下拉刷新列表 -->
  <van-pull-refresh class="h-full overflow-y-auto" v-model="refreshing" @refresh="getScoreRank">

    <!-- ===== 前三名领奖台展示 ===== -->
    <div v-if="top3List.length > 0" class="pt-4 pb-2 px-3 bg-gradient-to-b from-amber-50 to-white">
      <!-- 第1名（中间最高） -->
      <div class="flex items-end justify-center mb-2">
        <!-- 第2名（左边） -->
        <div v-if="top3List.length >= 2" class="flex flex-col items-center mx-2 mb-[-8px]">
          <div class="relative">
            <span class="text-3xl">🥈</span>
            <div v-if="top3List[1].IsCurrentUser" class="absolute -top-1 -right-2 bg-blue-500 text-white text-[10px] rounded-full px-1 font-bold">我</div>
          </div>
          <van-image round width="44px" height="44px" class="border-2 border-gray-300 mt-1" :src="top3List[1].Avatar" />
          <div class="text-xs font-bold text-gray-600 mt-1 truncate max-w-[80px] text-center">{{ top3List[1].Nickname }}</div>
          <div class="text-[10px] text-gray-400">答{{ top3List[1].AnswerCount }}题</div>
          <div class="text-xs font-bold text-gray-600">{{ top3List[1].RankScore }}分</div>
          <div class="w-[70px] h-[50px] bg-gradient-to-t from-gray-300 to-gray-200 rounded-t-lg mt-1 flex items-end justify-center pb-1">
            <span class="text-[10px] text-gray-500 font-bold">第2名</span>
          </div>
        </div>

        <!-- 第1名（中间最高） -->
        <div v-if="top3List.length >= 1" class="flex flex-col items-center mx-2 z-10">
          <div class="relative">
            <span class="text-4xl">👑</span>
            <span class="absolute -top-1 left-7 text-2xl">🥇</span>
            <div v-if="top3List[0].IsCurrentUser" class="absolute -top-1 -right-2 bg-blue-500 text-white text-[10px] rounded-full px-1 font-bold">我</div>
          </div>
          <van-image round width="52px" height="52px" class="border-2 border-amber-400 mt-1" :src="top3List[0].Avatar" />
          <div class="text-sm font-bold text-amber-700 mt-1 truncate max-w-[90px] text-center">{{ top3List[0].Nickname }}</div>
          <div class="text-[10px] text-amber-500">答{{ top3List[0].AnswerCount }}题 · 正确率{{ top3List[0].Accuracy }}%</div>
          <div class="text-base font-bold text-amber-600">{{ top3List[0].RankScore }}分</div>
          <div class="w-[80px] h-[70px] bg-gradient-to-t from-amber-400 to-amber-300 rounded-t-lg mt-1 flex items-end justify-center pb-1">
            <span class="text-xs text-white font-bold">第1名</span>
          </div>
        </div>

        <!-- 第3名（右边） -->
        <div v-if="top3List.length >= 3" class="flex flex-col items-center mx-2 mb-[-4px]">
          <div class="relative">
            <span class="text-3xl">🥉</span>
            <div v-if="top3List[2].IsCurrentUser" class="absolute -top-1 -right-2 bg-blue-500 text-white text-[10px] rounded-full px-1 font-bold">我</div>
          </div>
          <van-image round width="44px" height="44px" class="border-2 border-amber-200 mt-1" :src="top3List[2].Avatar" />
          <div class="text-xs font-bold text-orange-700 mt-1 truncate max-w-[80px] text-center">{{ top3List[2].Nickname }}</div>
          <div class="text-[10px] text-orange-400">答{{ top3List[2].AnswerCount }}题</div>
          <div class="text-xs font-bold text-orange-600">{{ top3List[2].RankScore }}分</div>
          <div class="w-[70px] h-[40px] bg-gradient-to-t from-amber-200 to-amber-100 rounded-t-lg mt-1 flex items-end justify-center pb-1">
            <span class="text-[10px] text-amber-600 font-bold">第3名</span>
          </div>
        </div>
      </div>
    </div>

    <!-- ===== 榜单列表头 ===== -->
    <div class="border-b border-solid border-[#ebedf080] bg-white sticky top-0 z-10">
      <van-cell value-class="inline-flex items-center">
        <template #value>
          <span class="w-10 flex-shrink-0 text-center font-medium">排名</span>
          <span class="w-[42px] flex-shrink-0 text-center font-medium">头像</span>
          <span class="w-full flex-grow text-left font-medium ml-1">用户名</span>
          <span class="w-[52px] flex-shrink-0 text-center font-medium">答题数</span>
          <span class="w-[52px] flex-shrink-0 text-center font-medium">正确率</span>
          <span class="w-[44px] flex-shrink-0 text-center font-medium">总分</span>
        </template>
      </van-cell>
    </div>

    <!-- ===== 榜单列表 ===== -->
    <div v-if="dataList.length == 0" class="text-center py-10 text-gray-400">暂无记录</div>

    <!-- 第4-10名 -->
    <template v-for="(item, index) in rank4to10List" :key="'r-' + index">
      <van-cell
        value-class="inline-flex items-center text-[#323233]"
        :class="{ 'bg-blue-50': item.IsCurrentUser }"
      >
        <template #value>
          <span class="w-10 flex-shrink-0 text-center font-bold text-lg">{{ item.Rank }}</span>
          <span class="w-[42px] h-[42px] flex-shrink-0 text-center relative">
            <van-image round width="42px" height="42px" :src="item.Avatar" />
            <div v-if="item.IsCurrentUser" class="absolute -top-1 -right-1 bg-blue-500 text-white text-[9px] rounded-full px-1 font-bold leading-tight">我</div>
          </span>
          <span class="w-full text-left overflow-hidden ml-1 flex items-center">
            <span class="truncate" :class="{ 'font-bold text-blue-600': item.IsCurrentUser }">{{ item.Nickname }}</span>
          </span>
          <span class="w-[52px] flex-shrink-0 text-center text-sm">{{ item.AnswerCount }}</span>
          <span class="w-[52px] flex-shrink-0 text-center text-sm">{{ item.Accuracy }}%</span>
          <span class="w-[44px] flex-shrink-0 text-center text-sm font-bold">{{ item.RankScore }}</span>
        </template>
      </van-cell>
    </template>

    <!-- ===== 自己排名（不在前10时单独展示） ===== -->
    <div v-if="myRankInfo" class="mt-4 mx-3 mb-4">
      <div class="text-center text-sm text-gray-400 mb-2">—— 我的排名 ——</div>
      <div class="flex items-center px-4 py-3 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-xl border border-blue-200">
        <span class="w-10 flex-shrink-0 text-center font-bold text-blue-600 text-lg">{{ myRankInfo.Rank }}</span>
        <span class="w-[46px] h-[46px] flex-shrink-0 text-center relative">
          <van-image round width="46px" height="46px" class="border-2 border-blue-300" :src="myRankInfo.Avatar" />
          <div class="absolute -top-1 -right-1 bg-blue-500 text-white text-[9px] rounded-full px-1 font-bold leading-tight">我</div>
        </span>
        <span class="w-full text-left overflow-hidden ml-2">
          <span class="font-bold text-blue-700 truncate block">{{ myRankInfo.Nickname }}</span>
          <span class="text-[10px] text-blue-400">共答题 {{ myRankInfo.AnswerCount }} 题 · 正确率 {{ myRankInfo.Accuracy }}%</span>
        </span>
        <span class="flex-shrink-0 text-center ml-2">
          <div class="text-xs text-gray-500">综合分</div>
          <div class="text-base font-bold text-blue-600">{{ myRankInfo.RankScore }}</div>
        </span>
      </div>
    </div>

  </van-pull-refresh>
</template>

<script setup lang="ts">
import http from '@http'
import { ref, computed, onBeforeMount } from 'vue'

const refreshing = ref(false)
const dataList = ref<any[]>([]) //排名列表集合

/** 前三名 */
const top3List = computed(() => dataList.value.filter(r => r.Rank >= 1 && r.Rank <= 3))

/** 第4-10名 */
const rank4to10List = computed(() => {
  return dataList.value.filter(r => r.Rank >= 4 && r.Rank <= 10)
})

/** 自己的排名（不在前10时） */
const myRankInfo = computed(() => {
  const me = dataList.value.find(r => r.IsCurrentUser)
  if (me && me.Rank > 10) return me
  return null
})

const onClickLeft = () => history.back()

/** 获取成绩榜单 */
const getScoreRank = () => {
  refreshing.value = true
  http
    .get('api/MockTest/ScoreRank')
    .right((res) => {
      dataList.value = res.Data
    })
    .finally(() => {
      refreshing.value = false
    })
}

onBeforeMount(() => {
  getScoreRank()
})
</script>
<style scoped>
.van-cell {
  align-items: center;
}
</style>
