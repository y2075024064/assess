<template>
  <el-pagination
    class="pager-pagination"
    layout="total, sizes, prev, pager, next, jumper"
    :current-page="currentPage"
    :page-sizes="pageSizes"
    :page-size="pageSize"
    :disabled="disabled"
    :pager-count="7"
    :total="total"
    background
    small
    @size-change="sizeChange"
    @current-change="currentChange"
  />
</template>

<script>
export default {
  name: "MyPager",
  props: {
    total: { type: Number, default: 0 },
    pageSize: { type: Number, default: 15 },
    currentPage: { type: Number, default: 1 },
    disabled: { type: Boolean, default: false },
  },
  emits: [
    "update:pageSize",
    "update:currentPage",
    "sizeChange",
    "currentChange",
  ],
  data() {
    var pageSizes = [15, 30, 50, 80, 100, 200, 500];
    return {
      pageSizes,
    };
  },
  methods: {
    sizeChange(size) {
      this.$emit("update:pageSize", size);
      this.$emit("sizeChange");
    },
    currentChange(page) {
      this.$emit("update:currentPage", page);
      this.$emit("currentChange");
    },
  },
};
</script>

<style lang="scss">
.pager-pagination {
  align-self: flex-end;
  padding: 5px 10px !important;
}
</style>
