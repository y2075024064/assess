<template>
  <el-dialog
    class="my-dialog"
    :show-close="false"
    :top="dialogData.top"
    :model-value="modelValue"
    :width="dialogData.width"
    :modal="dialogData.modal"
    :draggable="dialogData.draggable"
    :open-delay="dialogData.openDelay"
    :fullscreen="dialogData.fullscreen"
    :lock-scroll="dialogData.lockScroll"
    :close-delay="dialogData.closeDelay"
    :align-center="dialogData.alignCenter"
    :before-close="dialogData.beforeClose"
    :append-to-body="dialogData.appendToBody"
    :destroy-on-close="dialogData.destroyOnClose"
    :close-on-click-modal="dialogData.closeOnClickModal"
    :close-on-press-escape="dialogData.closeOnPressEscape"
    @open="dialogOpen"
    @close="dialogClose"
    @opened="dialogOpened"
    @closed="dialogClosed"
  >
    <template #header>
      <div class="dialog-header-content">
        <el-icon v-if="dialogData.icon">
          <component :is="dialogData.icon" />
        </el-icon>
        &nbsp;
        <span>{{ dialogData.title }}</span>
      </div>
      <div class="dialog-header-buttons">
        <el-icon v-if="dialogData.showMaximize" @click="onFullscreen">
          <Windowed v-if="dialogData.fullscreen" />
          <Maximized v-else />
        </el-icon>
        <el-icon v-if="dialogData.showClose" @click="closeDialog">
          <Close />
        </el-icon>
      </div>
    </template>
    <div>
      <slot></slot>
    </div>
  </el-dialog>
</template>

<script>
import { Close } from "@element-plus/icons-vue";
import Maximized from "@/svgs/maximized.vue";
import Windowed from "@/svgs/windowed.vue";

export default {
  name: "MyDialog",
  components: { Close, Maximized, Windowed },
  props: {
    modelValue: { type: Boolean, required: true },
    options: { type: Object, required: true },
  },
  emits: ["update:modelValue"],
  data() {
    var _options = {
      showClose: true,
      draggable: false,
      fullscreen: false,
      alignCenter: true,
      showMaximize: true,
      destroyOnClose: true,
      width: "fit-content",
      closeOnClickModal: false,
    };
    Object.assign(_options, this.options || {});
    return {
      dialogData: _options,
    };
  },
  methods: {
    closeDialog() {
      this.$emit("update:modelValue", false);
    },
    dialogOpen() {
      if (this.dialogData.open) {
        this.dialogData.open();
      }
    },
    dialogOpened() {
      this.$emit("update:modelValue", true);
      if (this.dialogData.opened) {
        this.dialogData.opened();
      }
    },
    dialogClose() {
      if (this.dialogData.close) {
        this.dialogData.close();
      }
    },
    dialogClosed() {
      this.$emit("update:modelValue", false);
      if (this.dialogData.closed) {
        this.dialogData.closed();
      }
    },
    onFullscreen() {
      this.dialogData.fullscreen = !this.dialogData.fullscreen;
    },
  },
};
</script>

<style lang="scss">
.el-overlay {
  width: 100%;
  position: absolute !important;

  .el-overlay-dialog {
    width: 100%;
    height: 100%;
    overflow: hidden;
    position: absolute !important;

    .my-dialog {
      display: flex;
      max-height: 100%;
      flex-direction: column;

      .el-dialog__header {
        display: flex;
        margin: 0px !important;
        padding: 15px !important;
        border-bottom: 1px solid #ddd;

        .dialog-header-content {
          width: 100%;
          display: flex;
          line-height: 21px;
          align-items: center;
        }

        .dialog-header-buttons {
          width: auto;
          display: flex;
          align-items: center;

          .el-icon {
            padding: 2px;
            margin: 0px 5px;
            cursor: pointer;
          }
        }
      }

      .el-dialog__body {
        height: 100%;
        overflow: auto;
        padding: 20px !important;
      }
    }
  }
}
</style>
