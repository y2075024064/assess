<template>
  <PCIframe v-if="isPC" />
  <Mobile v-else />
</template>

<script setup lang="ts">
import { useAppStore } from '@store/app'
import PCIframe from '@views/PCIframe.vue'
import Mobile from '@views/Mobile.vue'

var { isMobile } = useAppStore()
var isIframe = window.self !== window.top

const isPC = !isMobile && !isIframe
</script>

<style lang="css">
* {
  margin: 0px;
  padding: 0px;
}

/* H5的时候，隐藏滚动条 */
::-webkit-scrollbar {
  display: none;
}

body {
  width: 100vw;
  height: 100vh;
  --van-border-color: #c6c6c6;
  --van-cell-border-color: #c6c6c6;
}

#app {
  @apply w-full h-full flex flex-col items-center justify-center bg-[#eff2f5];
}
</style>
