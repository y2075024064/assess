<template>
  <van-nav-bar
    fixed
    placeholder
    left-arrow
    :title="typeName + '动态'"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
  />
  <van-list
    v-model:loading="loading"
    :finished="finished"
    finished-text="没有更多了"
    @load="onLoad"
  >
    <!-- 新闻 -->
    <van-cell-group v-if="type == '新闻'">
      <van-cell
        v-for="(item, index) in newsList"
        :value="item.PublishTime.substring(2, 10)"
        :label="item.NickName"
        :title="item.Title"
        size="large"
        :key="index"
        @click="toDynamicDetail(item.Id)"
      />
    </van-cell-group>
    <!-- 系统 -->
    <ul v-else class="p-[10px]">
      <li
        v-for="(item, index) in systemList"
        class="flex w-full my-[0.5rem] pb-[10px] cursor-pointer system"
        @click="toDynamicDetail(item.Id)"
        :key="index"
      >
        <van-image :src="item.PreviewImg" height="6rem" radius="0.5rem" fit="fill" width="40%" />
        <span class="text-[15px] my-[0.2rem] ml-[0.5rem]">{{ item.Title }}</span>
      </li>
    </ul>
  </van-list>
</template>

<script setup lang="ts">
import http from '@http'
import { computed, onBeforeMount, ref } from 'vue'
import { DynamicInfo } from '@/types/DynamicInfo'
import { useRoute, useRouter } from 'vue-router'

const router = useRouter()
const loading = ref(false)
const finished = ref(false)
const newsList = ref<DynamicInfo[]>([]) //当前新闻列表
const systemList = ref<DynamicInfo[]>([]) //当前通知列表
const type = ref('新闻')
const page = ref(1)
const take = ref(5)
const Total = ref(0)
const onClickLeft = () => history.back()
const route = useRoute()
const typeName = computed(() => (type.value == '系统' ? '通知' : type.value))

/** 根据类型获取全部 */
const onLoad = () => {
  loading.value = true
  const params = { page: page.value, take: take.value, type: type.value }
  http.get('api/Dynamic/GetDynamicList', { params }).right((res) => {
    Total.value = res.Total as number
    if (type.value == '新闻') {
      newsList.value = newsList.value.concat(res.Data)
    } else {
      systemList.value = systemList.value.concat(res.Data)
    }
    // 加载状态结束
    page.value++
    loading.value = false
    // 数据全部加载完成
    if (systemList.value.length >= Total.value) {
      finished.value = true
    } else if (newsList.value.length >= Total.value) {
      finished.value = true
    }
  })
}

/** 前往动态的详细界面 */
const toDynamicDetail = (id: number) => {
  router.push({ name: 'dynamicDetail', query: { id } })
}

onBeforeMount(() => {
  type.value = route.query.Type as string
})
</script>

<style scoped>
.ellip {
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 4;
  text-overflow: ellipsis;
  -webkit-box-orient: vertical;
}

::v-deep(.van-cell__title) {
  width: 100%;
}

::v-deep(.van-cell__value) {
  flex-shrink: 0;
  width: 60px;
  flex: none;
}

.system {
  border-bottom: 2px solid #c2bebe;
}
</style>
