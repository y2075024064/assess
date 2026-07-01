<template>
    <van-nav-bar 
        fixed 
        placeholder 
        left-arrow
        title="推送内容" 
        safe-area-inset-top 
        class="flex-shrink-0"     
        @click-left="onClickLeft"
    />
    <div v-html="adInfo" class="p-[0.3rem]"></div>
</template>

<script setup lang="ts">
import http from "@http"
import { onBeforeMount, ref } from "vue"
import { useRoute } from "vue-router";

const adInfo = ref()

const onClickLeft = () => history.back()

const getAdInfo = () => {
    const route = useRoute()
    const { id } = route.query        
    http
        .get('api/AdModule/AdContent', { params: { Id: id }})
        .right((res) => {
            adInfo.value = res.Data
        })
}

onBeforeMount(() => {
    getAdInfo()
})
</script>