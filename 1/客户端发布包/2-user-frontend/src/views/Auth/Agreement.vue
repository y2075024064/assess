<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="用户注册协议"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="urlBack"
  />
  <div class="text-center my-5">用户注册协议，待补充</div>
</template>

<script setup lang="ts">
const urlBack = () => history.back()
</script>
