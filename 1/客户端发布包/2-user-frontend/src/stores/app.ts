import { getItem, setItem, removeItem } from '@/utils/storage'
import { defineStore } from 'pinia'

const tokenKey = 'assessToken'
const rememberKey = 'assessRemember'

export const useAppStore = defineStore('app', {
  state: () => {
    // 获取当前设备类型
    let _isMobile = false
    let _clientType = 'PCH5'
    const userAgent = navigator.userAgent.toLowerCase()
    if (userAgent.indexOf('mobile') > -1) {
      _clientType = 'MobileH5'
      _isMobile = true
    }

    const token = getItem(tokenKey)
    const remember = getItem(rememberKey)

    return {
      token,
      remember,
      path: '/',
      dialogShowed: false,
      notidyShowed: false,
      isMobile: _isMobile,
      clientType: _clientType
    }
  },
  actions: {
    setToken(val: string, remember: boolean) {
      this.token = val
      this.remember = remember
      setItem(tokenKey, val)
      setItem(rememberKey, remember)
    },
    clearToken() {
      this.token = ''
      removeItem(tokenKey)
    }
  }
})
