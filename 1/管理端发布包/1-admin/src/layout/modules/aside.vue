<template>
  <el-aside width="auto">
    <el-menu
      active-text-color="#ffd04b"
      background-color="#20222a"
      class="aside-menu"
      text-color="#ddd"
      :default-openeds="defaultOpeneds"
      :default-active="selectedIndex"
      :collapse="isCollapse"
      @select="onSelect"
    >
      <el-menu-item class="aside-title" index="home">
        <el-icon
          class="aside-collapse-icon"
          title="侧边伸缩"
          @click="changeCollapse"
        >
          <Expand v-if="isCollapse" />
          <Fold v-else />
        </el-icon>
        <div v-if="!isCollapse" class="aside-title-txt">在线测评</div>
      </el-menu-item>
      <my-menu
        v-for="item in menus"
        :key="item.name"
        :node="item"
        @click="onItemClick"
      />
    </el-menu>
  </el-aside>
</template>

<script>
import { Expand, Fold } from "@element-plus/icons-vue";

export default {
  name: "LayoutAside",
  components: { Expand, Fold },
  props: {
    selectedIndex: { type: String, default: "" },
    menus: { type: Array, required: true },
  },
  emits: ["select", "menu-click"],
  data() {
    var isCollapse = this.$store.getters.collapse;
    var defaultOpeneds = [];
    for (var i = 0; i < this.menus.length; i++) {
      defaultOpeneds.push(this.menus[i].name);
    }
    return {
      isCollapse,
      defaultOpeneds,
    };
  },
  mounted() {
    if (this.menus.length) {
      this.defaultOpeneds.push(this.menus[0].name);
    }
  },
  methods: {
    onSelect(index, indexPath, item, routeResult) {
      this.$emit("select", index, indexPath, item, routeResult);
    },
    onItemClick(item, node) {
      this.$emit("menu-click", item, node);
    },
    changeCollapse() {
      this.isCollapse = !this.isCollapse;
      this.$store.dispatch("app/collapse", this.isCollapse);
    },
  },
};
</script>

<style lang="scss" scoped>
.aside-menu {
  height: 100%;

  .aside-title {
    height: 50px;
    color: #ddd;
    display: flex;
    flex-shrink: 0;
    cursor: default;
    line-height: 50px;
    padding: 0px !important;
    background-color: #363636;

    .aside-collapse-icon {
      width: 63px;
      height: 100%;
      flex-shrink: 0;
      cursor: pointer;
      font-size: 28px;
    }

    .aside-collapse-icon:hover {
      background-color: black;
    }

    .aside-title-txt {
      width: 100%;
      font-size: 20px;
      font-weight: 600;
      font-family: 楷体;
      text-align: center;
      padding-right: 15px;
    }
  }
}

.aside-menu:not(.el-menu--collapse) {
  width: 200px;
}
</style>
