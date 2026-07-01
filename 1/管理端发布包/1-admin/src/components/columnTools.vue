<template>
  <el-button v-if="showInfo" size="small" link @click="onInfo" type="info">
    {{ infoText }}
  </el-button>
  <el-button v-if="showEdit" size="small" link @click="onEdit" type="primary">
    {{ editText }}
  </el-button>
  <el-button v-if="showDel" size="small" link @click="onDelelte" type="danger">
    {{ delText }}
  </el-button>
</template>

<script>
export default {
  name: "ColumnTools",
  props: {
    showDel: { type: Boolean, default: true },
    showInfo: { type: Boolean, default: true },
    showEdit: { type: Boolean, default: true },
    delText: { type: String, default: "删除" },
    editText: { type: String, default: "编辑" },
    infoText: { type: String, default: "详情" },
  },
  emits: ["onDel", "onInfo", "onEdit"],
  data() {
    return {};
  },
  methods: {
    onInfo() {
      this.$emit("onInfo");
    },
    onEdit() {
      this.$emit("onEdit");
    },
    onDelelte() {
      this.$emit("onDel");
    },
  },
};
</script>
