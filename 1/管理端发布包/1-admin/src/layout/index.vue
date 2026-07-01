<template>
  <div class="layout-body">
    <el-container class="layout-container">
      <!-- 侧边栏 - 菜单栏 -->
      <Aside
        :menus="menus"
        :selectedIndex="menuSelectedItem"
        @menu-click="menuClick"
      />

      <el-main class="layout-main">
        <el-container class="layout-container">
          <!-- 页眉 -->
          <Header :refresh-tab="refreshTab" />

          <!-- 内容主体 -->
          <el-main class="layout-main">
            <el-tabs
              class="layout-tabs"
              type="card"
              v-model="tabsSelectedItem"
              @tab-change="tabChanged"
              @tab-remove="removeTab"
            >
              <!-- 此处为固定标签页 -->
              <el-tab-pane name="index" :closable="false">
                <template #label>
                  <span class="layout-tabs-label">
                    <el-icon><HomeFilled /></el-icon>
                    <span>首页</span>
                  </span>
                </template>
                <!-- 首页组件 -->
                <HomeView :key="indexTabIndex" />
              </el-tab-pane>

              <el-tab-pane
                v-for="item in mainTabs"
                :name="item.name"
                :key="item.name"
                :closable="true"
              >
                <template #label>
                  <span class="layout-tabs-label">
                    <el-icon><component :is="item.icon" /></el-icon>
                    <span>{{ item.title }}</span>
                  </span>
                </template>

                <!-- 动态组件 -->
                <component :is="item.component" :key="item.id" />
              </el-tab-pane>
            </el-tabs>

            <el-dropdown
              class="layout-tabs-dropdown"
              placement="bottom-end"
              trigger="hover"
              @command="tabsCommand"
            >
              <span class="tabs-dropdown-content">
                <el-icon><ArrowDown /></el-icon>
              </span>
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item command="closeCurrent">
                    关闭当前页
                  </el-dropdown-item>
                  <el-dropdown-item command="closeOther">
                    关闭其他页
                  </el-dropdown-item>
                  <el-dropdown-item command="closeAll">
                    关闭全部页
                  </el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </el-main>

          <!-- 页脚 -->
          <el-footer class="layout-footer" height="30px">
            &copy;2023&emsp;
            <a href="https://wwww.17gz8.com"> 青少年在线测评后台管理系统 </a>
            &emsp;&emsp; 技术支持&nbsp;
            <a href="https://www.cnblogs.com/sgwy"> @伟宇凌风 </a>
          </el-footer>
        </el-container>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import { HomeFilled, ArrowDown } from "@element-plus/icons-vue";
import HomeView from "@/views/Home/HomeView.vue";
import Header from "./modules/header.vue";
import Aside from "./modules/aside.vue";
import { getMenus } from "@/menus";
import { markRaw } from "vue";

export default {
  name: "LayoutIndex",
  components: { HomeFilled, ArrowDown, Aside, Header, HomeView },
  data() {
    var userType = this.$store.getters.type;
    var loadTime = new Date().getTime();
    var menus = getMenus(userType);
    return {
      menus,
      loadTime,
      mainTabs: [],
      menuSelectedItem: null,
      tabsSelectedItem: "index",
      indexTabIndex: "tab-key-0",
    };
  },
  created(){
    this.$store.dispatch("user/getInfo").catch(() => {
      console.log("获取用户信息失败（后端未启动）");
    });
  },
  mounted() {
    var userType = this.$store.getters.type;
    this.menus = getMenus(userType);
  },
  methods: {
    createTabKey() {
      var nowTime = new Date().getTime();
      var diff = nowTime - this.loadTime;
      return "tab-key-" + diff;
    },
    menuClick(item, node) {
      var _item = null;
      if (this.mainTabs.length) {
        _item = this.mainTabs.find((e) => e.name == node.name);
      }

      if (!_item) {
        if (node.component) {
          node.id = this.createTabKey();
          if (typeof node.component === "function") {
            node.component().then((module) => {
              node.component = markRaw(module.default);
              this.mainTabs.push(node);
            });
          } else {
            this.mainTabs.push(node);
          }
        }
      }
      this.tabsSelectedItem = node.name;
    },
    refreshTab() {
      var _name = this.tabsSelectedItem;
      if (_name == "index") {
        this.indexTabIndex = this.createTabKey();
      } else {
        var node = this.mainTabs.find((e) => e.name == _name);
        if (node) {
          node.id = this.createTabKey();
        }
      }
    },
    tabChanged(name) {
      this.menuSelectedItem = name;
    },
    removeTab(name) {
      this.mainTabs = this.mainTabs.filter((e) => e.name != name);
      if (this.tabsSelectedItem == name) {
        this.tabsSelectedItem = "index";
      }
    },
    tabsCommand(name) {
      var _name = this.tabsSelectedItem;
      switch (name) {
        case "closeCurrent":
          this.removeTab(_name);
          break;
        case "closeOther":
          var node = this.mainTabs.find((e) => e.name == _name);
          if (node) {
            this.mainTabs = [node];
          }
          break;
        case "closeAll":
          this.mainTabs = [];
          this.menuSelectedItem = "home";
          this.tabsSelectedItem = "index";
          break;
      }
    },
  },
};
</script>

<style lang="scss">
.layout-body {
  width: 100vw;
  height: 100vh;
  min-width: 800px;

  .layout-container {
    height: 100%;

    .layout-main {
      padding: 0;
      position: relative;

      .layout-tabs {
        height: 100%;
        border: none;
        display: flex;
        flex-direction: column;

        .el-tabs__header {
          flex-shrink: 0;
          height: 35px !important;
          padding: 5px 45px 0 10px;
          background-color: #ededed;
          margin-bottom: 0px !important;
          border-bottom: none !important;
          box-shadow: 0 0 10px #ccc inset;

          .is-scrollable {
            padding: 0 25px !important;
          }

          .el-tabs__nav {
            border: none !important;

            .el-tabs__item {
              height: 35px;
              line-height: 35px;
              border-left: none !important;

              .is-icon-close {
                top: 0px !important;
              }
            }

            .is-active {
              background-color: white;
              border-radius: 5px 5px 0 0;
            }
          }

          .el-tabs__nav-prev,
          .el-tabs__nav-next {
            font-size: 20px;
            line-height: 40px !important;
          }
        }

        .el-tabs__content {
          height: 100%;
          padding: 8px;
          background-color: white;

          .el-tab-pane {
            height: 100%;
            position: relative;
            border: 1px solid #ddd;
            box-shadow: 0px 2px 5px 2px #dadada;
          }
        }

        .layout-tabs-label {
          .el-icon {
            vertical-align: middle;
          }

          span {
            vertical-align: middle;
            margin-left: 4px;
          }
        }
      }

      .layout-tabs-dropdown {
        top: 0px;
        right: 0px;
        width: 40px;
        height: 40px;
        font-size: 20px;
        position: absolute;
        text-align: center;

        .tabs-dropdown-content {
          width: 100%;
          padding-top: 12px;
        }
      }
    }

    .layout-footer {
      font-size: 12px;
      line-height: 30px;
      text-align: center;
      box-sizing: border-box;
      background-color: whitesmoke;
      box-shadow: inset 0px 1px 5px 0px #d1d1d1;

      a {
        text-decoration: none;
      }
    }
  }
}
</style>
