<template>
  <el-sub-menu v-if="node.children" :index="node.name">
    <template #title>
      <el-icon v-if="node.icon"><component :is="node.icon" /></el-icon>
      <span>{{ node.title }}</span>
    </template>
    <my-menu
      v-for="item in node.children"
      :key="item.name"
      :node="item"
      @click="onSubItemClick"
    />
  </el-sub-menu>

  <el-menu-item v-else :index="node.name" @click="onThisClick">
    <el-icon v-if="node.icon"><component :is="node.icon" /></el-icon>
    <span>{{ node.title }}</span>
  </el-menu-item>
</template>

<script>
export default {
  name: "MyMenu",
  props: {
    node: { type: Object, required: true },
  },
  emits: ["click"],
  data() {
    return {};
  },
  methods: {
    onThisClick(item) {
      this.$emit("click", item, this.node);
    },
    onSubItemClick(item, node) {
      this.$emit("click", item, node);
    },
  },
};
</script>
