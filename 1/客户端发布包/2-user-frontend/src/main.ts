import 'vant/lib/index.css'
import './style/index.scss'
import 'vant/lib/button/index.css'

import Vant from 'vant'
import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

const app = createApp(App)
app.use(createPinia())
app.use(router)
app.use(Vant)

app.mount('#app')
