<template>
  <canvas
    ref="canvasRef"
    class="ver-code-canvas"
    :width="width"
    :height="height"
    :data-code="currentCode"
    @click="drawCode"
  />
</template>

<script>
export default {
  name: "VerificationCode",
  props: {
    width: { type: Number, default: 120 },
    height: { type: Number, default: 38 },
    codeLength: { type: Number, default: 4 },
  },
  emits: ["update:code"],
  data() {
    return {
      currentCode: "",
    };
  },
  mounted() {
    this.drawCode();
  },
  methods: {
    // 生成随机颜色
    randomColor(min = 30, max = 180) {
      var r = Math.floor(Math.random() * (max - min) + min);
      var g = Math.floor(Math.random() * (max - min) + min);
      var b = Math.floor(Math.random() * (max - min) + min);
      return `rgb(${r},${g},${b})`;
    },
    // 生成随机验证码
    generateCode() {
      var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
      var code = "";
      for (var i = 0; i < this.codeLength; i++) {
        code += chars.charAt(Math.floor(Math.random() * chars.length));
      }
      return code;
    },
    // 绘制验证码
    drawCode() {
      var canvas = this.$refs.canvasRef;
      if (!canvas) return;
      var ctx = canvas.getContext("2d");

      // 生成新验证码
      this.currentCode = this.generateCode();
      this.$emit("update:code", this.currentCode);

      // 清空画布
      ctx.clearRect(0, 0, this.width, this.height);

      // 背景 - 渐变色
      var gradient = ctx.createLinearGradient(0, 0, this.width, this.height);
      gradient.addColorStop(0, "#f0f4ff");
      gradient.addColorStop(0.5, "#e8f0fe");
      gradient.addColorStop(1, "#f5f0ff");
      ctx.fillStyle = gradient;
      ctx.fillRect(0, 0, this.width, this.height);

      // 干扰线 (3~5条)
      var lineCount = Math.floor(Math.random() * 3) + 3;
      for (var l = 0; l < lineCount; l++) {
        ctx.beginPath();
        ctx.moveTo(
          Math.random() * this.width,
          Math.random() * this.height
        );
        ctx.lineTo(
          Math.random() * this.width,
          Math.random() * this.height
        );
        ctx.strokeStyle = this.randomColor(100, 200);
        ctx.lineWidth = Math.random() * 1.5 + 0.5;
        ctx.stroke();
      }

      // 绘制字符
      var charArr = this.currentCode.split("");
      var charWidth = this.width / (this.codeLength + 1);
      var startX = charWidth * 0.6;

      for (var i = 0; i < charArr.length; i++) {
        var x = startX + i * charWidth;
        var y = this.height / 2 + Math.random() * 6 - 3;

        ctx.font = `bold ${Math.floor(Math.random() * 6 + 20)}px "Arial", sans-serif`;
        ctx.textBaseline = "middle";
        ctx.textAlign = "center";

        // 文字颜色
        ctx.fillStyle = this.randomColor(20, 130);

        // 轻微旋转
        var angle = (Math.random() - 0.5) * 0.4;
        ctx.save();
        ctx.translate(x, y);
        ctx.rotate(angle);
        ctx.fillText(charArr[i], 0, 0);
        ctx.restore();
      }

      // 干扰点 (20~40个)
      var dotCount = Math.floor(Math.random() * 21) + 20;
      for (var d = 0; d < dotCount; d++) {
        ctx.beginPath();
        ctx.arc(
          Math.random() * this.width,
          Math.random() * this.height,
          Math.random() * 2.5 + 0.5,
          0,
          Math.PI * 2
        );
        ctx.fillStyle = this.randomColor(120, 200);
        ctx.fill();
      }
    },
    // 对外暴露：获取当前验证码
    getCode() {
      return this.currentCode;
    },
  },
};
</script>

<style lang="scss" scoped>
.ver-code-canvas {
  display: block;
  border-radius: 0 8px 8px 0;
  cursor: pointer;
  border: 1.5px solid #e2e8f0;
  border-left: none;
  transition: box-shadow 0.3s ease;

  &:hover {
    box-shadow: 0 0 0 1.5px #4ecdc4 inset;
  }
}
</style>
