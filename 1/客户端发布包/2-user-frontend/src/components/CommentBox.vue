<template>
  <!-- 评论列表 手风琴样式  -->
  <van-list
    v-model:loading="loading"
    :finished="finished"
    finished-text="没有更多了"
    @load="getCommentList"
  >
    <div class="flex flex-col grow mt-[10px] mb-[40px] min-h-20">
      <div class="text-[14px] leading-[14px] font-bold bg-white p-[0.5rem]">
        {{ totalComment ? '共 ' + totalComment + ' 条回复' : '回复列表' }}
      </div>
      <div
        v-if="!commentList.length"
        class="flex flex-col grow items-center justify-center bg-white text-[#9b9b9b]"
      >
        当前暂无评论
      </div>
      <van-collapse v-model="activeName" accordion>
        <van-collapse-item
          :name="index"
          v-for="(item, index) in commentList"
          :key="index"
          class="mb-[0.5rem]"
        >
          <template #title>
            <div class="w-full flex">
              <div class="flex-none justify-center items-center">
                <van-image height="40" width="40" fit="contain" radius="25" :src="item.Avatar" />
              </div>
              <div class="flex-1 relative ml-[8px]">
                <div class="text-[14px]">{{ item.Nickname }}</div>
                <div class="text-[12px] text-[#000]">
                  {{ item.CreationTime }} ·
                  <span class="text-[#9c9c9c]" @click.stop="showResp(item.Id)">回复</span>
                </div>
              </div>
            </div>
            <div class="relative my-[0.2rem]">
              <div>{{ item.Content }}</div>
              <div class="absolute right-[-1rem] top-[0.3rem]" @click.stop="deleteComment(item.Id)">
                <van-icon name="delete-o" />
              </div>
            </div>
            <ul
              v-for="(img, index) in item.Images"
              :key="index"
              @click="previewImg(item.Images, index)"
            >
              <van-image width="10rem" height="6rem" fit="cover" radius="0.5rem" :src="img" />
            </ul>
          </template>
          <!-- 子评论列表 -->
          <Comment :commentList="item.Children" @change-show="showResp" />
        </van-collapse-item>
      </van-collapse>
    </div>
    <!-- 回复菜单 -->
    <div
      class="w-full h-[80px] flex gap-[1rem] px-[1rem] items-center fixed bottom-[0px] bg-white py-[10px]"
    >
      <div
        class="flex grow w-m-[50%] bg-[#ccc] text-[#efefef] rounded-[25px] pl-[15px] py-[5px]"
        @click="showResp()"
      >
        说点什么吧
      </div>
    </div>
  </van-list>
  <!-- 回复窗 -->
  <van-popup v-model:show="showInput" safe-area-inset-bottom position="bottom" round>
    <div class="p-[10px]">
      <textarea
        placeholder="在这里输入内容"
        class="min-h-[200px] max-h-[60vh] w-full"
        :value="content"
        @input="setContent($event)"
      >
      </textarea>
      <van-cell-group style="padding: 10px 10px 2px">
        <van-uploader v-model="contentImgs" multiple :max-count="3" />
      </van-cell-group>
      <button
        class="absolute right-8 bottom-4 bg-orange-400 text-white py-[5px] px-[1rem] rounded-[5px]"
        @click="addComments"
      >
        发布
      </button>
    </div>
  </van-popup>
</template>

<script setup lang="ts">
import http from '@http'
import { ref, onMounted, watch } from 'vue'
import { ResponseInfo } from '@/types/ResponseInfo'
import { showImagePreview, showNotify } from 'vant'
import Comment from '@components/Comment.vue'

const commentList = ref<ResponseInfo[]>([]) // 当前问题的回复列表
const showInput = ref(false) // 输入窗是否打开
const content = ref('') // 评论内容
const contentImgs = ref<any[]>([]) // 评论图片
const curCommentId = ref(0) // 当前回复的评论或子评论的id
const activeName = ref('0')
const page = ref(1)
const take = 10
const loading = ref(false)
const finished = ref(false)
const totalComment = ref(0)

const props = defineProps<{
  objectId: number
}>()
// const isLike = ref(false)

/** 更新用户输入内容 */
const setContent = (event: any) => {
  content.value = event.target.value
}

/** 获取当前对象的评论信息 */
const getCommentList = () => {
  loading.value = true
  http
    .post('/api/Comment/List', {
      Page: page.value,
      Take: take,
      OrderType: 'time',
      ObjectId: props.objectId,
      ObjectType: 'Letter'
    })
    .right((res) => {
      commentList.value = commentList.value.concat(res.Data)
      totalComment.value = res.Total as number
      // 加载状态结束
      page.value++
      loading.value = false
      // 数据全部加载完成
      if (commentList.value.length >= totalComment.value) {
        finished.value = true
      }
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}

/** 打开评论回复窗口，并设置回复的id */
const showResp = (commentId: number = 0) => {
  curCommentId.value = commentId
  showInput.value = true
}

/** 发布评论 */
const addComments = () => {
  const formData = new FormData()
  let objectType = 'Letter'
  formData.append('objectId', props.objectId + '')
  if (curCommentId.value) {
    formData.append('commentId', curCommentId.value + '')
    objectType = 'Comment'
  }
  formData.append('objectType', objectType)
  formData.append('content', content.value)
  for (const item of contentImgs.value) {
    formData.append('images', item.file)
  }
  http
    .postForm('/api/Comment/Insert', formData)
    .right((res) => {
      showInput.value = false
      content.value = ''
      contentImgs.value = []
      commentList.value = []
      page.value = 1
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'success'
      })
      getCommentList()
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}

/** 删除评论 */
const deleteComment = (commentId: number) => {
  http
    .delete('/api/Comment/Delete', { params: { id: commentId } })
    .right((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'success'
      })
      commentList.value = commentList.value.filter((obj) => obj.Id !== commentId)
      totalComment.value -= 1
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}

/** 点赞 */
// const like = (type: string, objectId: number | undefined) => {
//   http
//     .get('/api/Like/Insert', { params: { objectId, type } })
//     .right((res) => {
//       isLike.value = true
//       showNotify({
//         message: res.Msg,
//         duration: 2000,
//         type: 'success'
//       })
//     })
//     .error((res) => {
//       showNotify({
//         message: res.Msg,
//         duration: 2000,
//         type: 'danger'
//       })
//     })
// }

/** 图片预览 */
const previewImg = (imgs: string[], index: number) => {
  showImagePreview({
    images: imgs,
    startPosition: index
  })
}

onMounted(() => {
  getCommentList()
})
</script>
