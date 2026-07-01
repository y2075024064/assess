<template>
  <el-header class="layout-header" height="50px">
    <ul class="header-nav nav-left">
      <li title="刷新当前选项卡">
        <el-icon class="nav-icon" @click="refreshTab">
          <Refresh />
        </el-icon>
      </li>
    </ul>

    <ul class="header-nav nav-right">
      <li title="便签">
        <el-popover
          placement="bottom"
          trigger="click"
          :width="400"
          @hide="onHide"
        >
          <template #reference>
            <el-icon class="nav-icon">
              <Shortnote />
            </el-icon>
          </template>

          <el-input
            type="textarea"
            :rows="5"
            v-model="shortnoteTxt"
            :placeholder="placeholder"
          />
        </el-popover>
      </li>

      <li :title="isFullscreen ? '退出全屏' : '全屏'">
        <el-icon class="nav-icon" @click="screenFullOrNofull">
          <ExitFullscreen v-if="isFullscreen" />
          <Fullscreen v-else />
        </el-icon>
      </li>

      <li title="我的"><LayoutUser /></li>

      <li title="版本信息">
        <el-icon class="nav-icon" @click="showVersionInfo = true">
          <MoreFilled class="icon-more" />
        </el-icon>
      </li>
    </ul>

    <el-drawer
      size="350px"
      title="版本信息"
      :with-header="true"
      v-model="showVersionInfo"
    >
      <span>青少年计算思维在线测评后台管理系统 V1.0.0.0</span>
    </el-drawer>
  </el-header>
</template>

<script>
import { Refresh, MoreFilled } from "@element-plus/icons-vue";
import { getCookies, setCookies } from "@/utils/auth";
import LayoutUser from "./user.vue";

import ExitFullscreen from "@/svgs/exitFullscreen.vue";
import Fullscreen from "@/svgs/fullscreen.vue";
import Shortnote from "@/svgs/shortnote.vue";

export default {
  name: "LayoutHeader",
  components: {
    Refresh,
    MoreFilled,
    ExitFullscreen,
    Fullscreen,
    Shortnote,
    LayoutUser,
  },
  props: {
    refreshTab: { type: Function, default: function () {} },
  },
  data() {
    var shortnoteTxt = getCookies("69No_ms_shortnote");
    return {
      shortnoteTxt,
      isFullscreen: false,
      showVersionInfo: false,
      placeholder:
        "便签中的内容会存储在本地，这样即便你关掉了浏览器，在下次打开时，依然会读取到上一次的记录。是个非常小巧实用的本地备忘录",
    };
  },
  methods: {
    onHide() {
      setCookies(this.shortnoteTxt, true, "69No_ms_shortnote");
    },
    screenFullOrNofull() {
      if (this.isFullscreen) {
        this.isFullscreen = false;
        if (document.exitFullScreen) {
          document.exitFullscreen();
        }
        //兼容火狐
        else if (document.mozCancelFullScreen) {
          document.mozCancelFullScreen();
        }
        //兼容谷歌等
        else if (document.webkitExitFullscreen) {
          document.webkitExitFullscreen();
        }
        //兼容IE
        else if (document.msExitFullscreen) {
          document.msExitFullscreen();
        } else {
          this.isFullscreen = true;
        }
      } else {
        this.isFullscreen = true;
        if (document.documentElement.RequestFullScreen) {
          document.documentElement.RequestFullScreen();
        }
        //兼容火狐
        else if (document.documentElement.mozRequestFullScreen) {
          document.documentElement.mozRequestFullScreen();
        }
        //兼容谷歌等webkitRequestFullScreen
        else if (document.documentElement.webkitRequestFullScreen) {
          document.documentElement.webkitRequestFullScreen();
        }
        //兼容IE,只能写msRequestFullscreen
        else if (document.documentElement.msRequestFullscreen) {
          document.documentElement.msRequestFullscreen();
        } else {
          this.isFullscreen = false;
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.layout-header {
  box-sizing: border-box;
  padding: 0px 0px 0px 10px !important;
  box-shadow: 0px 1px 2px 1px #cfcfcf;

  .header-nav {
    margin: 0px;
    height: 100%;
    padding: 0px;
    list-style: none;

    li {
      float: left;
      height: 100%;

      .nav-icon {
        width: 50px;
        height: 100%;
        cursor: pointer;
        font-size: 20px;
        color: #999999;
      }

      .nav-icon:hover {
        color: black;
      }

      .icon-more {
        transform: rotate(0.25turn);
      }
    }
  }

  .nav-left {
    float: left;
  }

  .nav-right {
    float: right;
  }
}
</style>
