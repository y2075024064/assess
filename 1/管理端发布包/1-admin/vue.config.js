const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  assetsDir: 'static',
  publicPath: '/',
  devServer: {
    open: true,
    port: 9527,
    proxy: {
      '/admin': {
        target: 'http://localhost:5177',
        changeOrigin: true,
        pathRewrite: {
          '^/admin': ''
        }
      },
      '/api': {
        target: 'http://localhost:5177',
        changeOrigin: true
      },
      '/Auth': {
        target: 'http://localhost:5177',
        changeOrigin: true
      },
      '/Home': {
        target: 'http://localhost:5177',
        changeOrigin: true
      }
    }
  },
  configureWebpack: {
    devtool: 'source-map'
  },
  chainWebpack: config => {
    config
      .plugin('html')
      .tap(args => {
        args[0].title = "青少年在线测评平台后台管理系统"
        return args
      })
  }
})
