<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="发布信件"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
  />

  <van-form ref="formRef">
    <van-cell-group>
      <van-field
        clearable
        label="标题"
        :label-width="50"
        placeholder="请输入标题"
        v-model="formModel.title"
        :rules="[
          { required: true, message: '请填写标题' },
          { validator: titleValidator, message: '标题长度不得小于5个字！' }
        ]"
      />
    </van-cell-group>

    <van-cell-group>
      <van-field
        rows="2"
        autosize
        label="内容"
        type="textarea"
        maxlength="200"
        show-word-limit
        :label-width="50"
        placeholder="请输入内容"
        v-model="formModel.content"
        :rules="[
          { required: true, message: '请填写内容' },
          { validator: contentValidator, message: '内容长度不得小于15个字！' }
        ]"
      />
    </van-cell-group>

    <van-cell-group style="padding: 10px 16px 2px">
      <van-uploader v-model="imgList" multiple :max-count="3" />
    </van-cell-group>

    <van-cell-group style="padding: 10px 16px">
      <van-checkbox
        v-model="formModel.isPublic"
        checked-color="#EA580C"
        class="text-[15px]"
        icon-size="17"
      >
        公开信件
      </van-checkbox>
    </van-cell-group>

    <van-cell-group>
      <van-field :label-width="50" label="标签">
        <template #input>
          <div class="w-full pt-[2px]">
            <van-checkbox-group v-model="labelSeted" direction="horizontal">
              <van-checkbox v-for="item in labels" :key="item" :name="item" class="mb-2">
                <template #icon="{ checked }">
                  <div class="h-full flex items-center">
                    <van-tag round size="medium" :type="checked ? 'primary' : 'default'">
                      {{ item }}
                    </van-tag>
                  </div>
                </template>
              </van-checkbox>
            </van-checkbox-group>

            <div class="flex justify-between mt-1">
              <div v-if="showAdd">
                <input v-model="newLabel" class="add" @keyup.enter="addLabel" />
                &nbsp;
                <van-button type="primary" size="mini" @click="addLabel">添 加</van-button>
                &nbsp;
                <van-button type="default" size="mini" @click="cancelAdd">取 消</van-button>
              </div>
              <div v-else class="cursor-pointer" @click="showAdd = true">标签 +</div>

              <div class="text-[13px] text-gray-500">
                {{ labelSeted.length }} / {{ labels.length }}
              </div>
            </div>
          </div>
        </template>
      </van-field>
    </van-cell-group>
  </van-form>

  <div class="fixed text-center bottom-8 w-full">
    <van-button class="w-[60%]" style="background-color: rgb(251, 146, 60)" @click="submit">
      确定
    </van-button>
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify } from 'vant'
import { onBeforeMount, ref } from 'vue'

const onClickLeft = () => history.back()

const formRef = ref()
const newLabel = ref('') //新添加的标签
const showAdd = ref(false) //显示标签添加的输入框
const imgList = ref<any[]>([]) //信件中添加的图片

const labels = ref(['竞赛']) //所有的标签
const labelSeted = ref<string[]>([]) //所有被选中的标签

const formModel = ref<Record<string, any>>({
  title: '', //信件的标题
  content: '', //信件的详细内容
  isPublic: true //信件是否公开
})

//标题验证
const titleValidator = (val: string) => val.length > 4
//内容验证
const contentValidator = (val: string) => val.length > 14

// 添加标签
const addLabel = () => {
  showAdd.value = false
  labels.value.push(newLabel.value)
  labelSeted.value.push(newLabel.value)
  newLabel.value = ''
}

// 取消添加标签
const cancelAdd = () => {
  showAdd.value = false
  newLabel.value = ''
}

/** 获取初始标签 */
const getInitLabels = () => {
  http.get('api/Tag/Hots').right((res) => {
    labels.value = res.Data
  })
}

// 提交及判断
const submit = () => {
  formRef.value.validate().then(() => {
    const formData = new FormData()
    for (const item in formModel.value) {
      formData.append(item, formModel.value[item])
    }
    for (const item of imgList.value) {
      formData.append('images', item.file)
    }
    for (const item of labelSeted.value) {
      formData.append('label', item)
    }

    http
      .postForm('api/Letter/Insert', formData)
      .right((res) => {
        showNotify({
          message: res.Msg,
          type: 'success',
          duration: 2000
        })
        history.back()
      })
      .error((res) => {
        showNotify({
          message: res.Msg,
          type: 'danger',
          duration: 2000
        })
      })
  })
}

onBeforeMount(() => getInitLabels())
</script>

<style lang="scss" scoped>
.van-cell-group {
  margin-bottom: 10px;
}

.add {
  width: 80px;
  padding: 2px 3px;
  border-bottom: 1px solid #000;
  font-size: 12px;
}
</style>
