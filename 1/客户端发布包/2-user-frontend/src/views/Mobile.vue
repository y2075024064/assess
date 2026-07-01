<template>
  <div :class="$style['main-container']">
    <LoadingAndError v-if="loading || isError" :error="isError" :loading="loading" />
    <router-view v-else />
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { showToast } from 'vant'
import { RouterView } from 'vue-router'
import { useAppStore } from '@store/app'
import LoadingAndError from '@components/LoadingAndError.vue'

import { useRoute } from 'vue-router'
import { ref, computed } from 'vue'

const route = useRoute()
const loading = ref(true)
const isError = ref(false)
const isLoginPage = computed(() => route.path === '/login' || route.path === '/regist')

var appStore = useAppStore()
if (!appStore.token && !isLoginPage.value) {
  http
    .get('Home/GetToken')
    .right((res) => {
      appStore.setToken(res.Data, true)
      location.href = '/login'
    })
    .error((res) => {
      showToast({
        message: res.Msg,
        duration: 2000,
        type: 'fail'
      })
    })
    .catch((err) => {
      isError.value = true
    })
    .finally(() => {
      loading.value = false
    })
} else {
  loading.value = false
}
</script>

<style lang="css" module>
.main-container {
  @apply w-full h-full flex flex-col bg-[#f7f8fa];
}
</style>
