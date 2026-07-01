<template>
  <div :class="$style['mobile-box']">
    <iframe class="w-full h-full rounded-[20px]" :src="href" title="mobile" frameborder="0" />
  </div>
</template>

<script setup lang="ts">
import '@vant/touch-emulator'
const href = location.href
</script>

<style lang="css" module>
.mobile-box {
  @apply w-2/5 h-[85%] min-w-[360px] min-h-[466px] max-w-[420px] rounded-[20px] box-border drop-shadow-lg;
}
</style>
