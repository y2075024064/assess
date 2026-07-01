<template>
  <div v-if="!commentList.length">暂无人评论</div>
  <div v-for="(item, index) in commentList" :key="index" class="bg-white px-4 pt-4 text-black">
    <div class="w-full flex">
      <div class="flex-none justify-center items-center">
        <van-image height="40" width="40" fit="contain" radius="25" :src="item.Avatar" />
      </div>
      <div class="flex-1 relative ml-[8px]">
        <div class="text-[14px]">{{ item.Nickname }}</div>
        <div class="text-[12px] text-[#000]">{{ item.CreationTime }} · 回复</div>
        <van-tag type="warning" plain class="absolute top-0 right-0" v-if="true">
          {{ item.MemberType }}
        </van-tag>
      </div>
    </div>
    <div>{{ item.Content }}</div>
    <ul v-for="(img, index) in item.Images" :key="index" @click="previewImg(item.Images, index)">
      <van-image width="10rem" height="6rem" fit="cover" radius="0.5rem" :src="img" />
    </ul>
  </div>
</template>

<script setup lang="ts">
import { showImagePreview } from 'vant'
import { ResponseInfo } from '@/types/ResponseInfo'

defineProps<{
  commentList: Array<ResponseInfo>
}>()

defineEmits(['changeShow'])

const previewImg = (imgs: string[], index: number) => {
  showImagePreview({
    images: imgs,
    startPosition: index
  })
}
</script>
