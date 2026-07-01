<template>
  <el-select
    :model-value="modelValue"
    :placeholder="placeholder"
    :remote-method="remoteMethod"
    :allow-create="allowCreate"
    :multiple="multiple"
    :loading="loading"
    remote-show-suffix
    reserve-keyword
    filterable
    clearable
    remote
    @change="onChange"
  >
    <el-option
      v-for="(item, index) in options"
      :value="item.Value"
      :label="item.Text"
      :key="index"
    />
  </el-select>
</template>

<script>
export default {
  name: "MySelect",
  props: [
    "modelValue",
    "url",
    "initOption",
    "placeholder",
    "multiple",
    "allowCreate",
  ],
  emits: ["update:modelValue"],
  data() {
    return {
      options: [],
      loading: false,
    };
  },
  mounted() {
    this.remoteMethod("");
  },
  methods: {
    onChange(value) {
      this.$emit("update:modelValue", value);
    },
    remoteMethod(query) {
      this.loading = true;
      var params = { q: query };
      this.$http.get(this.url, { params }).then((res) => {
        this.loading = false;
        if (res.IsSucc) {
          this.options = res.Data;
        }
      });
    },
  },
  watch: {
    initOption: function (val) {
      this.options.push(val);
    },
  },
};
</script>
