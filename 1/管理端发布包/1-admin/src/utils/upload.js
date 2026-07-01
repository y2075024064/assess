import axios from "./request";

/**
 * 上传文件
 * @param {string} url 上传地址
 * @param {FormData} formData 表单信息
 * @param {Function} succFunc 成功回调
 * @param {Function} errFunc 错误回调
 */
function uploadFile(url, formData, succFunc, errFunc) {
  axios.postForm(url, formData).then((res) => {
    if (res.IsSucc) {
      succFunc(res.Data);
    } else {
      errFunc(res.Msg);
    }
  });
}

/**
 * 上传图片
 * @param {File} file 文件对象
 * @param {string} type 上传类型
 * @param {Function} succFunc 成功回调
 * @param {Function} errFunc 错误回调
 */
export function UploadImage(file, type, succFunc, errFunc) {
  if (file.size > 3145728) {
    errFunc("图片不得超过3M");
  } else {
    var formData = new FormData();
    formData.append("ImageFile", file);
    var url = `admin/Upload/Image?type=${type}`;
    uploadFile(url, formData, succFunc, errFunc);
  }
}

/**
 * 上传视频
 * @param {File} file 文件对象
 * @param {string} type 上传类型
 * @param {Function} succFunc 成功回调
 * @param {Function} errFunc 错误回调
 */
export function UploadVideo(file, type, succFunc, errFunc) {
  if (file.size > 20971520) {
    errFunc("视频不得超过20M");
  } else {
    var formData = new FormData();
    formData.append("VideoFile", file);
    var url = `admin/Upload/Video?type=${type}`;
    uploadFile(url, formData, succFunc, errFunc);
  }
}
