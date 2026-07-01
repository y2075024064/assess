<template>
    <van-nav-bar fixed placeholder left-arrow title="计算思维之星" safe-area-inset-top class="flex-shrink-0"
        @click-left="onClickLeft" />
    <div class="flex flex-col grow h-full overflow-x-hidden overflow-y-auto">
        <!-- 详细 -->
        <div class="p-[10px] bg-white">
            <div class="flex font-bold text-[18px]">
                <van-image width="6rem" height="6rem" fit="cover" radius="0.5rem" :src="thinkStarInfo?.PreviewImg"
                    @click="previewOneImg(thinkStarInfo!?.PreviewImg)" />
                <div class="flex-1 p-[1rem]">
                    <div>计算思维之星</div>
                    <div class="text-[16px]">{{ thinkStarInfo?.Title }}</div>
                </div>
            </div>
            <div class="my-[0.3rem]" v-html="thinkStarInfo?.Content"></div>
        </div>
    </div>
</template>

<script setup lang="ts">
import http from '@http'
import { ref, onBeforeMount } from "vue"
import { useRoute } from "vue-router"
import { ThinkStarInfo } from "@/types/ThinkStarInfo";
import { showImagePreview, showNotify } from 'vant'

const onClickLeft = () => history.back()
const route = useRoute()
const thinkStarInfo = ref<ThinkStarInfo>() // 当前问题的详细信息


/** 获取思维之星的详细信息 */
const getThinkStarInfo = () => {
    const { id } = route.query
    http
        .get('/api/ThinkStar/Detail', { params: { id: id } })
        .right((res) => {
            thinkStarInfo.value = res.Data
        })
        .error((res) => {
            showNotify({
                message: res.Msg,
                duration: 2000,
                type: 'danger'
            })
        })
}

/** 单张图片预览 */
const previewOneImg = (img: string) => {
    const imgs = []
    imgs.push(img)
    showImagePreview({
        images: imgs
    });
}

onBeforeMount(() => {
    getThinkStarInfo()
})
</script>