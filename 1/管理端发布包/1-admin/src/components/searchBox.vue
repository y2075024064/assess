<template>
  <el-input
    clearable
    :model-value="modelValue"
    :placeholder="placeholder"
    :style="{ width: width || '200px' }"
    @keyup.enter="onClick"
    @input="onChange"
  >
    <template #append>
      <el-button @click="onClick" :icon="searchIcon" />
    </template>
  </el-input>
  <input type="text" style="display: none" />
</template>

<script>
import { Search } from "@element-plus/icons-vue";
import { markRaw } from "vue";

export default {
  name: "SearchBox",
  props: ["modelValue", "width", "placeholder"],
  emits: ["update:modelValue", "search"],
  data() {
    return {
      searchIcon: markRaw(Search),
    };
  },
  methods: {
    onChange(value) {
      this.$emit("update:modelValue", value);
    },
    onClick() {
      this.$emit("search");
    },
  },
};
</script>
