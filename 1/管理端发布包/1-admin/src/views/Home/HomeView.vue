<template>
  <div class="welcome-txt">欢迎使用青少年计算思维在线测评后台管理系统</div>
</template>

<script>
export default {
  name: "HomeView",
  data() {
    return {
      dateTime: new Date().getMilliseconds(),
    };
  },
};
</script>

<style lang="scss" scoped>
.welcome-txt {
  font-size: 25px;
  text-align: center;
  padding-top: 100px;
  font-family: "楷体";
}
</style>
