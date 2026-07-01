import { defineStore } from 'pinia'
import http from '@http'

export interface ChatMessage {
  role: 'user' | 'assistant'
  content: string
  time: string
}

export const useMinaiStore = defineStore('minai', {
  state: () => {
    return {
      /** 是否显示米奈浮动宠物 */
      showPet: true,
      /** 是否打开聊天弹窗 */
      showChat: false,
      /** 宠物位置 */
      posX: window.innerWidth - 100,
      posY: window.innerHeight - 200,
      /** 对话历史 */
      messages: [] as ChatMessage[],
      /** 是否正在加载AI回复 */
      isLoading: false,
      /** 是否首次打开 */
      isFirstOpen: true
    }
  },
  actions: {
    /** 切换宠物显示 */
    togglePet() {
      this.showPet = !this.showPet
    },
    /** 隐藏宠物 */
    hidePet() {
      this.showPet = false
    },
    /** 显示宠物 */
    showMinai() {
      this.showPet = true
    },
    /** 打开聊天 */
    openChat() {
      this.showChat = true
      if (this.isFirstOpen) {
        this.addMessage('assistant', '你好呀！我是「米奈」✨，你的专属AI学习伙伴！有什么想问我的吗？😊')
        this.isFirstOpen = false
      }
    },
    /** 关闭聊天 */
    closeChat() {
      this.showChat = false
    },
    /** 更新位置 */
    setPosition(x: number, y: number) {
      this.posX = x
      this.posY = y
    },
    /** 添加消息 */
    addMessage(role: 'user' | 'assistant', content: string) {
      const now = new Date()
      const time = `${now.getHours().toString().padStart(2, '0')}:${now.getMinutes().toString().padStart(2, '0')}`
      this.messages.push({ role, content, time })
    },
    /** 发送消息到AI */
    sendMessage(content: string) {
      if (!content.trim() || this.isLoading) return

      this.addMessage('user', content)
      this.isLoading = true

      const historyMessages = this.messages
        .slice(-10)
        .map(m => ({ role: m.role, content: m.content }))

      http
        .post('/ai/api/Chat/Send', { messages: historyMessages }, { baseURL: '' })
        .right((res) => {
          this.addMessage('assistant', res.Data.reply ?? res.Data.Reply ?? '嗯嗯~ 你说得对！😊')
        })
        .error(() => {
          this.addMessage('assistant', '唔…我好像走神了，可以再说一遍吗？🤔')
        })
        .catch(() => {
          this.addMessage('assistant', '哎呀，网络好像出了点问题…等会儿再找我聊吧~ 🐾')
        })
        .finally(() => {
          this.isLoading = false
        })
    },
    /** 清空对话 */
    clearMessages() {
      this.messages = []
      this.isFirstOpen = true
    }
  }
})
