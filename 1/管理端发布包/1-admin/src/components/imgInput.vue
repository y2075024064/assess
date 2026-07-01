<template>
  <div class="img-input">
    <div class="img-input_target" @click="click">
      <div v-if="previewSrc" class="img-body">
        <img :src="previewSrc" />
      </div>
      <div v-else class="img-icon">
        <Plus />
      </div>
      <input
        :id="id"
        type="file"
        :name="name"
        :accept="accept"
        :multiple="false"
        @change="fileChange"
      />
    </div>
    <div class="img-input__tip">图片最大 {{ size }}MB</div>
  </div>
</template>

<script>
import { Plus } from "@element-plus/icons-vue";

export default {
  name: "ImgInput",
  components: { Plus },
  props: {
    size: { type: Number, default: 0 },
    name: { type: String, default: "" },
    src: { type: String, default: null },
    accept: { type: String, default: "image/*" },
    modelValue: { type: Object, default: null },
  },
  computed: {
    previewSrc() {
      return this.imageSrc || this.src;
    },
  },
  emits: ["update:modelValue", "change"],
  data() {
    return {
      id: new Date().getTime().toString(),
      imageSrc: null,
    };
  },
  methods: {
    click() {
      document.getElementById(this.id).click();
    },
    fileChange(el) {
      var file = el.target.files[0];
      if (file.type.indexOf("image") < 0) {
        this.$message.error("上传的文件只能是 图片!");
      } else if (this.size > 0 && file.size / 1048576 > this.size) {
        this.$message.error(`上传图片大小不能超过 ${this.size}MB!`);
      } else {
        this.imageSrc = URL.createObjectURL(file);
        this.$emit("update:modelValue", file);
        this.$emit("change", file);
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.img-input {
  width: 100%;
  height: auto;
  display: flex;
  flex-direction: column;

  .img-input_target {
    width: 100%;
    height: 100%;
    display: flex;
    cursor: pointer;
    overflow: hidden;
    border: 1px dashed;
    border-radius: 6px;
    align-items: center;
    justify-content: center;
    border-color: var(--el-border-color);

    .img-body {
      width: 100%;
      height: 100%;

      img {
        width: 100%;
        border-style: none;
        object-fit: contain;
      }
    }

    .img-icon {
      width: 80%;
      height: 80%;
      padding: 10%;
      max-width: 100px;
      max-height: 100px;

      svg {
        width: 80%;
        height: 100%;
        margin: 0 auto;
        display: block;
      }
    }

    input[type="file"] {
      display: none;
    }
  }

  .img-input_target:hover {
    border-color: var(--el-border-color-hover);
  }

  .img-input__tip {
    font-size: 12px;
    line-height: 12px;
    padding: 7px 0 3px 3px;
  }
}
</style>
