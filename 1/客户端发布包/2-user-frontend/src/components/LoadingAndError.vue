<template>
  <div class="loading-error-box">
    <div class="error-box" v-if="error">
      <div class="icon-box">
        <van-icon name="warn-o" size="50" />
      </div>
      网络异常
    </div>
    <van-loading v-else-if="loading" vertical type="spinner" size="50" color="#1989fa">
      加载中...
    </van-loading>
  </div>
</template>

<script lang="ts">
export default {
  props: ['loading', 'error'],
  setup() {
    return {}
  }
}
</script>

<style lang="scss" scoped>
.loading-error-box {
  width: 100%;
  height: 100%;
  display: -webkit-flex;
  display: flex;
  align-items: center;
  justify-content: center;

  .error-box {
    color: red;

    .icon-box {
      text-align: center;
    }
  }
}
</style>
