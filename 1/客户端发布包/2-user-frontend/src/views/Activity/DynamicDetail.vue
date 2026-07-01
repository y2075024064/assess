<template>
    <van-nav-bar fixed placeholder left-arrow title="新闻动态" safe-area-inset-top class="flex-shrink-0"
        @click-left="onClickLeft" />
    <div v-html="dynamicInfo" class="p-[0.3rem]"></div>

</template>

<script setup lang="ts">
import http from "@http"
import { onBeforeMount, ref } from "vue"
import { useRoute } from 'vue-router'


const onClickLeft = () => history.back()

const dynamicInfo = ref("")

/** 获取详细内容 */
const getDynamicDetail = () => {
    const route = useRoute()
    let { id } = route.query
    http.get('api/Dynamic/Detail', {
        params: {
            id
        }
    }).right((res) => {
        dynamicInfo.value = res.Data.Content
    })
}

onBeforeMount(() => {
    getDynamicDetail()
})
</script>

<style scoped></style>