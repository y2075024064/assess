<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="我的成绩"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
  />

  <!-- <div class="flex-shrink-0 border-b border-solid border-[#ebedf080]">
    <van-field
      is-link
      readonly
      clearable
      label="练习类别"
      v-model="typeFieldText"
      placeholder="选择练习类别"
      @click="showTypePicker = true"
    />

    <van-popup v-model:show="showTypePicker" safe-area-inset-bottom position="bottom" round>
      <van-picker
        :columns-field-names="{ text: 'Text', value: 'Value' }"
        :columns="typeColumns"
        @confirm="onTypeConfirm"
        @cancel="showTypePicker = false"
      />
    </van-popup>
  </div> -->

  <!-- 成绩列头 -->
  <div class="flex-shrink-0 border-b border-solid border-[#ebedf080]">
    <van-cell value-class="inline-flex items-center">
      <template #value>
        <span class="w-full flex-grow text-left">练习类别</span>
        <span class="w-[55px] flex-shrink-0 text-center">成绩</span>
        <span class="w-[55px] flex-shrink-0 text-center">难度</span>
        <span class="w-[100px] flex-shrink-0 text-right">答题时间</span>
      </template>
    </van-cell>
  </div>

  <!-- 成绩下拉刷新列表 -->
  <van-pull-refresh class="h-full overflow-y-auto" v-model="refreshing" @refresh="onRefresh">
    <van-list
      v-model:loading="loading"
      finished-text="没有更多了"
      :finished="finished"
      @load="onLoad"
    >
      <van-cell
        value-class="inline-flex items-center text-[#323233]"
        v-for="item in dataList"
        :key="item.Id"
      >
        <template #value>
          <span class="w-full flex-grow text-left overflow-hidden">
            {{ item.PartName }}
          </span>
          <span class="w-[55px] flex-shrink-0 text-center">{{ item.CurrScore }}</span>
          <span class="w-[55px] flex-shrink-0 text-center">
            {{ item.AvgDifficulty.toFixed(1) }}
          </span>
          <span class="w-[100px] flex-shrink-0 text-right">
            {{ dateTimeFormat(item.OverTime) }}
          </span>
        </template>
      </van-cell>
    </van-list>
  </van-pull-refresh>
</template>

<script setup lang="ts">
import http from '@http'
import { ref, onBeforeMount } from 'vue'

const loading = ref(false) //列表当前是否正在加载
const finished = ref(false) //列表当前是否加载完全
const refreshing = ref(false)
const dataList = ref<any[]>([]) //当前列表集合
const typeFieldText = ref('') //类别显示名称
const typeFieldValue = ref<number | null>(null) //类别ID
const showTypePicker = ref(false) //类别选择器是否显示
const typeColumns = ref([{ Text: '', Value: 0 }]) //练习类别选项

let page = 1
const take = 30

/** 时间格式化 */
const dateTimeFormat = (val: string) => {
  if (val) return val.substring(2, 16)
  else return ''
}

const onClickLeft = () => history.back()

/** 下拉刷新 */
const onRefresh = () => {
  // 清空列表数据
  page = 1
  dataList.value = []
  // 重新加载数据
  finished.value = false
}

/** 用户确认选择练习类别时 */
const onTypeConfirm = (event: any) => {
  var item = event.selectedOptions[0]
  showTypePicker.value = false

  if (typeFieldValue.value != item.Value) {
    typeFieldText.value = item.Text
    typeFieldValue.value = item.Value

    onRefresh()
  }
}

/** 当前加载列表下一页时 */
const onLoad = () => {
  var params = { page, take, partId: typeFieldValue.value }
  http
    .get('api/MockTest/ScoreList', { params })
    .right((res) => {
      const array = res.Data as Array<any>
      if (array.length > 0) {
        array.forEach((e) => dataList.value.push(e))
        ++page
      } else {
        finished.value = true
      }
    })
    .finally(() => {
      loading.value = false
      if (refreshing.value) refreshing.value = false
    })
}

/** 获取练习类别列表 */
var getTypeList = () => {
  http.get('api/TopicPart/List').right((res) => {
    typeColumns.value = res.Data
    typeFieldValue.value = typeColumns.value[0].Value
    onLoad()
  })
}

onBeforeMount(() => {
  //getTypeList()
})
</script>
