<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="我的收藏"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
    @click-right="onClickRight"
  >
    <template #right>
      <van-icon name="plus" size="18px" />
    </template>
  </van-nav-bar>

  <div class="text-center">
    <van-collapse v-model="activeIndex" accordion>
      <van-cell title-class="text-left" value-class="mr-5">
        <template #title>
          我的收藏夹
          <span class="text-g text-gray-400">&nbsp;·&nbsp;&nbsp;{{ total }}</span>
        </template>
        <template #right-icon>
          <van-icon class="ml-1 text-base text-transparent" name="arrow-down" />
        </template>
        <template #value>
          <span
            v-if="total > 0"
            class="text-xs cursor-pointer hover:text-blue-500"
            @click="gotoTest('')"
          >
            去练习 >>
          </span>
        </template>
      </van-cell>

      <van-collapse-item
        v-for="(item, index) in groupNames"
        title-class="text-left"
        value-class="mr-5"
        :name="index"
        :key="index"
        icon="minus"
      >
        <template #title>
          &nbsp;{{ item.name }}
          <span class="text-g text-gray-400">&nbsp;·&nbsp;&nbsp;{{ item.total }}</span>
        </template>
        <template #value>
          <span
            v-if="item.total > 0"
            class="text-xs cursor-pointer hover:text-blue-500"
            @click="gotoTest(item.name)"
          >
            去练习 >>
          </span>
        </template>
        <van-space class="w-full" wrap>
          <van-tag
            v-for="(number, index2) in item.numbers"
            class="cursor-pointer px-1"
            type="primary"
            :key="number"
            size="large"
            closeable
            plain
            @click="gotoPage(number)"
            @close="unCollecte(index, index2)"
          >
            <span class="w-[115px] text-center">{{ number }}</span>
          </van-tag>
        </van-space>
      </van-collapse-item>
    </van-collapse>
  </div>

  <van-dialog
    v-model:show="showGroupName"
    show-cancel-button
    title="添加分组"
    @confirm="addGroupNameConfirm"
  >
    <div class="mt-4 mb-3">
      <van-field v-model="groupName" label="名称" placeholder="请输入分组名称" label-width="auto" />
    </div>
  </van-dialog>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify } from 'vant'
import { useRouter } from 'vue-router'
import { ref, onBeforeMount } from 'vue'

/**分组信息 */
type GroupInfo = { name: string; total: number; numbers: Array<string> }

const router = useRouter()

const total = ref(0) //总收藏数
const groupName = ref('') //新增的分组名称
const activeIndex = ref(0) //当前手风琴选择项
const showGroupName = ref(false) //是否显示新增分组弹窗
const groupNames = ref<GroupInfo[]>([]) //用户收藏分组列表

const onClickLeft = () => history.back()
const onClickRight = () => (showGroupName.value = true)

/**跳转到题目详情界面 */
const gotoPage = (number: string) => {
  router.push({ name: 'testInfo', query: { number } })
}

/**跳转到练习界面 */
const gotoTest = (groupName: string) => {
  router.push({ name: 'practice', query: { type: 'collecte', groupName } })
}

/**确认添加分组名称 */
const addGroupNameConfirm = () => {
  var params = { name: groupName.value }
  http
    .get('api/Member/AddGroupName', { params })
    .right((res) => {
      getGroupNames()
      showNotify({
        message: res.Msg,
        type: 'success',
        duration: 2000
      })
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

/**获取用户当前所有分组 */
const getGroupNames = () => {
  http.get('api/Member/GetGroupNames').right((res) => {
    groupNames.value.splice(0)
    var array = res.Data as Array<string>
    for (var i = 0; i < array.length; i++) {
      groupNames.value.push({ name: array[i], total: 0, numbers: [] })
    }

    getCollecteList()
  })
}

/**获取用户收藏列表 */
const getCollecteList = () => {
  http.get('api/TopicCollecte/TotalInfo').right((res) => {
    total.value = res.Data.Total
    var array = res.Data.Groups as Array<any>
    for (var i = 0; array.length; i++) {
      var name = array[i].GroupName as string
      var numbers = array[i].TopicNumbers as Array<string>
      var item = groupNames.value.find((item) => item.name == name)
      if (item) {
        item.numbers = numbers
        item.total = numbers.length
      }
    }
  })
}

/**取消收藏 */
const unCollecte = (index1: number, index2: number) => {
  var number = groupNames.value[index1].numbers[index2]
  http
    .get('api/TopicCollecte/UnCollecte?number=' + number)
    .right((res) => {
      --total.value
      --groupNames.value[index1].total
      groupNames.value[index1].numbers.splice(index2, 1)
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

onBeforeMount(() => {
  getGroupNames()
})
</script>
