<template>
  <el-config-provider :locale="locale">
    <router-view />
  </el-config-provider>
</template>

<script>
import zhCn from "element-plus/dist/locale/zh-cn.min.mjs"

export default {
  name: 'App',
  data() {
    return { locale: zhCn };
  },
}
</script>

<style lang="scss">
* {
  margin: 0px;
  padding: 0px;
}

body {
  width: 100vw;
  height: 100vh;
}

#app {
  width: 100%;
  height: 100%;
}

.pager-container {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;

  .pager-tools {
    width: 100%;
    flex-shrink: 0;
    padding: 10px 0px 0px 10px;

    .el-form-item {
      margin-bottom: 10px !important;
    }
  }

  .el-form--inline {
    .el-form-item {
      margin-right: 15px;
    }
  }

  .pager-table {
    width: 100%;
    height: 100%;
  }
}

.el-img-select {
  width: 100%;

  .el-upload {
    width: 100%;
    transition: 0.2s;
    overflow: hidden;
    position: relative;
  }
}

input[type="number"] {

  &::-webkit-outer-spin-button,
  &::-webkit-inner-spin-button {
    -webkit-appearance: none;
  }

  -moz-appearance: textfield;
}
</style>
