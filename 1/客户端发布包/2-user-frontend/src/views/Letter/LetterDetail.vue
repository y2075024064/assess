<template>
  <van-nav-bar
    fixed
    placeholder
    left-arrow
    title="问题详情"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
  />
  <div class="h-full flex-grow overflow-x-hidden overflow-y-auto">
    <!-- 详细 -->
    <div class="p-[10px] bg-white">
      <div class="font-bold text-[20px]">{{ letterInfo?.Title }}</div>
      <div class="w-full flex my-[0.3rem]">
        <div class="flex-none justify-center items-center">
          <!-- 此处头像怎么获取？ -->
          <van-image height="40" width="40" fit="contain" radius="25" :src="letterInfo?.Avatar" />
        </div>
        <div class="flex-1 relative ml-[8px]">
          <div class="text-[14px]">{{ letterInfo?.Nickname }}</div>
          <div class="text-[12px] text-[#000]">{{ letterInfo?.CreationTime }} · 发布</div>
          <!--  -->
          <van-tag
            type="warning"
            plain
            class="absolute top-0 right-0 py-[2px]"
            v-if="letterInfo?.IsModel"
          >
            精华
          </van-tag>
          <van-tag type="warning" plain class="absolute top-0 right-0 py-[2px]" v-else>
            {{ states[letterInfo!.State] }}
          </van-tag>
        </div>
      </div>
      <div class="my-[0.3rem]">{{ letterInfo?.Content }}</div>
      <!-- 问题的图片列表 -->
      <ul
        v-for="(img, index) in letterInfo?.Images"
        :key="index"
        @click="previewImg(letterInfo!?.Images, index)"
      >
        <van-image width="10rem" height="6rem" fit="cover" radius="0.5rem" :src="img" />
      </ul>
      <!-- 信件的标签 -->
      <template v-if="letterInfo?.Label">
        <van-tag
          type="warning"
          round
          class="mr-[8px]"
          v-for="(label, LIndex) in JSON.parse(letterInfo!?.Label)"
          :key="LIndex"
        >
          {{ label }}
        </van-tag>
      </template>
    </div>
    <!-- 评论列表 -->
    <CommentBox :objectId="letterInfo!.Id" />
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { ref, onBeforeMount } from 'vue'
import { useRoute } from 'vue-router'
import { LetterInfo } from '@/types/LetterInfo'
import { showImagePreview, showNotify } from 'vant'
import CommentBox from '@components/CommentBox.vue'

const onClickLeft = () => history.back()
const route = useRoute()
const letterInfo = ref<LetterInfo>() // 当前问题的详细信息

const states: any = {
  New: '新增',
  Viewed: '已查看',
  Processing: '处理中',
  Resolved: '已解决',
  Finished: '已完成'
}

/** 获取当前信件的详细信息 */
const getLetterInfo = () => {
  const { id } = route.query
  http
    .get('/api/Letter/Detail', { params: { id: id } })
    .right((res) => {
      letterInfo.value = res.Data
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}
/** 图片预览 */
const previewImg = (imgs: string[], index: number) => {
  showImagePreview({
    images: imgs,
    startPosition: index
  })
}

onBeforeMount(() => {
  getLetterInfo()
})
</script>
