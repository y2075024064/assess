<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="联系我们"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="urlBack"
  />
  <div class="text-center my-5">联系我们信息，待补充。。。</div>
</template>

<script setup lang="ts">
const urlBack = () => history.back()
</script>
