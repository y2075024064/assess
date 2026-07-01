import { defineStore } from 'pinia'

interface UserInfo {
  gender: string
  avatar: string
  name: string
  type: string
}

export const useUserStore = defineStore('user', {
  state: () => {
    return {
      isLogin: false,
      gender: '保密',
      avatar: '',
      name: '',
      type: ''
    }
  },
  actions: {
    setUserInfo(info: UserInfo) {
      if (info) {
        this.type = info.type
        this.name = info.name
        this.avatar = info.avatar
        this.gender = info.gender
        this.isLogin = true
      }
    }
  }
})
