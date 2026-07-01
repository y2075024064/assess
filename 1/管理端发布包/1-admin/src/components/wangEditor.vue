<template>
  <div class="editor-box" :style="{ width: width, 'z-index': zIndex }">
    <Toolbar
      style="border-bottom: 1px solid #ccc"
      :defaultConfig="toolbarConfig"
      :editor="editor"
      :mode="mode"
      ref="toolbar"
    />
    <Editor
      :style="{ height: height, 'overflow-y': 'hidden' }"
      :defaultConfig="editorConfig"
      :model-value="modelValue"
      :mode="mode"
      @onCreated="handleCreated"
      @onChange="onChange"
    />
  </div>
</template>

<script>
import "@wangeditor/editor/dist/css/style.css"; // 引入 css

import { UploadImage, UploadVideo } from "@/utils/upload";
import { Editor, Toolbar } from "@wangeditor/editor-for-vue";

export default {
  name: "WangEditor",
  components: { Editor, Toolbar },
  props: {
    type: { type: String, required: true },
    modelValue: { type: String, default: "" },
    mode: { type: String, default: "simple" },
    width: { type: String, default: "100%" },
    height: { type: String, default: "320px" },
    shouldVideo: { type: Boolean, default: false },
    shouldImage: { type: Boolean, default: false },
    shouldFormula: { type: Boolean, default: false },
  },
  emits: ["update:modelValue"],
  expose: ["verifyImages", "getText"],
  data() {
    var toolbarKeys = [
      "undo",
      "redo",
      "blockquote",
      "fontSize",
      "|",
      "bold",
      "underline",
      "italic",
      "through",
      "code",
      "sup",
      "sub",
      "color",
      "bgColor",
      "clearStyle",
      "|",
      "bulletedList",
      "numberedList",
      "justifyLeft",
      "justifyRight",
      "justifyCenter",
      "indent",
      "delIndent",
      "|",
      "insertTable",
      "codeBlock",
      "|",
      "fullScreen",
    ];

    var editorConfig = {
      placeholder: "请输入内容...",
      hoverbarKeys: {},
      MENU_CONF: {},
    };

    if (this.shouldImage) {
      var index = toolbarKeys.length - 4;
      toolbarKeys.splice(index, 0, {
        key: "group-image",
        title: "图片",
        iconSvg:
          '<svg viewBox="0 0 1024 1024"><path d="M959.877 128l0.123 0.123v767.775l-0.123 0.122H64.102l-0.122-0.122V128.123l0.122-0.123h895.775zM960 64H64C28.795 64 0 92.795 0 128v768c0 35.205 28.795 64 64 64h896c35.205 0 64-28.795 64-64V128c0-35.205-28.795-64-64-64zM832 288.01c0 53.023-42.988 96.01-96.01 96.01s-96.01-42.987-96.01-96.01S682.967 192 735.99 192 832 234.988 832 288.01zM896 832H128V704l224.01-384 256 320h64l224.01-192z"></path></svg>',
        menuKeys: ["insertImage", "uploadImage"],
      });

      editorConfig.MENU_CONF["insertImage"] = {
        onInsertedImage: (imageNode) => {
          if (imageNode == null) return;
          this.added_imageList.push(imageNode.src);
        },
      };

      editorConfig.MENU_CONF["uploadImage"] = {
        maxNumberOfFiles: 10,
        customUpload: async (file, insertFn) => {
          UploadImage(
            file,
            this.type,
            (url) => insertFn(url, "", ""),
            this.$message.error
          );
        },
      };
    }

    if (this.shouldVideo) {
      index = toolbarKeys.length - 4;
      toolbarKeys.splice(index, 0, {
        key: "group-video",
        title: "视频",
        iconSvg:
          '<svg viewBox="0 0 1024 1024"><path d="M981.184 160.096C837.568 139.456 678.848 128 512 128S186.432 139.456 42.816 160.096C15.296 267.808 0 386.848 0 512s15.264 244.16 42.816 351.904C186.464 884.544 345.152 896 512 896s325.568-11.456 469.184-32.096C1008.704 756.192 1024 637.152 1024 512s-15.264-244.16-42.816-351.904zM384 704V320l320 192-320 192z"></path></svg>',
        menuKeys: ["insertVideo", "uploadVideo"],
      });

      editorConfig.MENU_CONF["uploadVideo"] = {
        maxNumberOfFiles: 1,
        customUpload: async (file, insertFn) => {
          UploadVideo(
            file,
            this.type,
            (url) => insertFn(url, ""),
            this.$message.error
          );
        },
      };
    }

    if (this.shouldFormula) {
      index = toolbarKeys.length - 2;
      toolbarKeys.splice(index, 0, "insertFormula");
      editorConfig.hoverbarKeys["formula"] = { menuKeys: ["editFormula"] };
    }

    return {
      zIndex: 1,
      editor: null,
      editorConfig,
      toolbarConfig: { toolbarKeys },
      added_imageList: [],
    };
  },
  methods: {
    onChange(_editor) {
      const html = _editor.getHtml();
      this.$emit("update:modelValue", html);
    },
    handleCreated(_editor) {
      this.editor = Object.seal(_editor);
      this.editor.on("fullScreen", () => {
        this.zIndex = 1000;
      });
      this.editor.on("unFullScreen", () => {
        this.zIndex = 1;
      });
    },
    verifyImages() {
      var delList = [];
      var now_imageList = this.editor.getElemsByType("image");
      for (var i = 0; i < this.added_imageList.length; i++) {
        var item = this.added_imageList[i];
        var index = now_imageList.findIndex((e) => e.src == item);
        if (index < 0) delList.push(item);
      }

      if (delList.length > 0) {
        this.deleteImages(delList);
      }
    },
    deleteImages(imgs) {
      this.$http.post(`admin/Upload/DeleteImg?type=${this.type}`, imgs);
    },
    getText() {
      return this.editor.getText();
    },
  },
  beforeUnmount() {
    if (this.editor == null) return;
    this.editor.destroy(); // 组件销毁时，及时销毁编辑器
  },
};
</script>

<style lang="scss" scoped>
.editor-box {
  border: 1px solid #ccc;
}
</style>
