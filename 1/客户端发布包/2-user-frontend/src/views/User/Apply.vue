<template>
  <van-nav-bar fixed left-arrow placeholder title="测评报名" safe-area-inset-top class="flex-shrink-0"
    @click-left="onClickLeft" />
  <div class="py-5 text-center">该功能开发中，敬请期待！</div>
</template>

<script setup lang="ts">
const onClickLeft = () => history.back()


</script>
