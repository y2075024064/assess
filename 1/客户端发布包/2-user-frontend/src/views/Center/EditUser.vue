<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="编辑信息"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="urlBack"
  />

  <div>
    <van-cell-group>
      <van-cell title-class="text-center">
        <template #title>
          <van-uploader :after-read="uploadAvatar" :preview-image="false" name="avatar">
            <van-image
              class="border border-solid border-gray-200 cursor-pointer"
              :src="userInfo.Avatar"
              height="80"
              width="80"
              round
            />
          </van-uploader>
        </template>
      </van-cell>
      <van-cell
        is-link
        title="账号"
        value-class="mr-2"
        :value="userInfo.Account"
        @click="openModifyDialog('Account')"
      />
      <van-cell
        is-link
        title="昵称"
        value-class="mr-2"
        :value="userInfo.Nickname"
        @click="openModifyDialog('Nickname')"
      />
      <van-cell
        is-link
        title="性别"
        value-class="mr-2"
        :value="userInfo.Gender"
        @click="showGenderPopup = true"
      />
      <van-cell
        is-link
        title="生日"
        value-class="mr-2"
        :value="userInfo.Birthday"
        @click="showBirthdayPopup = true"
      />
    </van-cell-group>
  </div>

  <van-dialog
    v-model:show="showModifyDialog"
    show-cancel-button
    title="更改信息"
    @confirm="modifyConfirmed"
  >
    <div class="my-4 px-3">
      <van-field
        class="border-b border-solid border-gray-300"
        v-model="modifyFieldValue"
        placeholder="请输入更改值"
        :label="modifyFieldName"
        label-width="auto"
      />
    </div>
  </van-dialog>

  <van-popup v-model:show="showGenderPopup" safe-area-inset-bottom position="bottom" round>
    <van-picker
      :columns="genderList"
      @confirm="genderConfirmed"
      @cancel="showGenderPopup = false"
    />
  </van-popup>

  <van-popup v-model:show="showBirthdayPopup" safe-area-inset-bottom position="bottom" round>
    <van-date-picker
      title="选择日期"
      :min-date="minDate"
      :max-date="maxDate"
      v-model="currBirthday"
      @confirm="birthdayConfirmed"
      @cancel="showBirthdayPopup = false"
    />
  </van-popup>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify } from 'vant'
import { ref, onBeforeMount } from 'vue'
import { UserInfo } from '@/types/UserInfo'
import type { UploaderFileListItem } from 'vant/lib/uploader/types'

type FieldName = 'Account' | 'Nickname'
let modifyField: FieldName = 'Account' //当前更改的字段

let nowY = new Date().getFullYear()
const minDate = new Date(nowY - 60, 0, 1)
const maxDate = new Date(nowY - 5, 11, 31)
const maxSize = 2 * 1024 * 1024 //头像最大大小

const currBirthday = ref([(nowY - 11).toString(), '01', '01'])
const modifyFieldName = ref('') //更改字段名称
const modifyFieldValue = ref('') //更改字段值
const showGenderPopup = ref(false) //是否显示性别下拉框
const showBirthdayPopup = ref(false) //是否显示生日下拉框
const showModifyDialog = ref(false) //是否显示字段更改弹窗
const userInfo = ref<UserInfo>(new UserInfo()) //用户信息
const genderList = ref([
  { text: '男', value: '男' },
  { text: '女', value: '女' },
  { text: '保密', value: '保密' }
])

const urlBack = () => history.back()

/** 上传头像 */
const uploadAvatar = (info: UploaderFileListItem | UploaderFileListItem[]) => {
  var file: File | undefined
  if (Array.isArray(info)) {
    file = info[0].file
  } else {
    file = info.file
  }

  if (file) {
    if (file.size > maxSize) {
      showErrorMsg('文件大小不能超过2M', 'warning')
      return
    }

    var formData = new FormData()
    formData.append('Value', '*')
    formData.append('avatar', file)
    http
      .postForm('api/Member/EditAvatar', formData)
      .right((res) => (userInfo.value.Avatar = res.Data))
      .error((res) => showErrorMsg(res.Msg))
  }
}

/** 打开字段更改弹窗 */
const openModifyDialog = (name: FieldName) => {
  modifyField = name
  modifyFieldValue.value = userInfo.value[name]
  switch (name) {
    case 'Account':
      if (!userInfo.value.CanModifyAccount) return
      modifyFieldName.value = '账户：'
      break
    case 'Nickname':
      modifyFieldName.value = '昵称：'
      break
  }
  showModifyDialog.value = true
}

/** 确认更改字段时 */
const modifyConfirmed = () => {
  const oldValue = userInfo.value[modifyField]
  const Value = modifyFieldValue.value

  if (Value && Value !== oldValue) {
    http
      .post(`api/Member/Edit${modifyField}`, { Value })
      .right(() => (userInfo.value[modifyField] = Value))
      .error((res) => showErrorMsg(res.Msg))
  }
}

/** 确认性别选择 */
const genderConfirmed = (option: any) => {
  var value = option.selectedValues[0]

  showGenderPopup.value = false
  http
    .get(`api/Member/EditGender?gender=${value}`)
    .right(() => (userInfo.value.Gender = value))
    .error((res) => showErrorMsg(res.Msg))
}

/** 确认生日选择 */
const birthdayConfirmed = (option: any) => {
  var value = option.selectedValues as Array<string>
  var Value = value.join('-')

  showBirthdayPopup.value = false
  http
    .post('api/Member/EditBirthday', { Value })
    .right(() => (userInfo.value.Birthday = Value))
    .error((res) => showErrorMsg(res.Msg))
}

/** 显示错误信息 */
const showErrorMsg = (msg: string, type: 'danger' | 'warning' = 'danger') => {
  showNotify({ message: msg, type: type, duration: 2000 })
}

onBeforeMount(() => {
  userInfo.value = history.state as UserInfo
  if (userInfo.value.Birthday) {
    currBirthday.value = userInfo.value.Birthday.split('-')
  }
})
</script>
