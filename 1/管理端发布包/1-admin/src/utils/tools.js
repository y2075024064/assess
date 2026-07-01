// Blob流文件下载 数据+文件名
// 注：只适合小文件的下载 大文件下载建议直接走url下载
export function downloadFile(data, name) {
  if (!data) {
    return;
  }

  var bytes = window.atob(data);
  var buffer = new ArrayBuffer(bytes.length);
  var array = new Uint8Array(buffer);
  for (var i = 0; i < bytes.length; i++) {
    array[i] = bytes.charCodeAt(i);
  }

  const url = window.URL.createObjectURL(new Blob([buffer]));
  const link = document.createElement("a");
  link.style.display = "none";
  link.download = name;
  link.href = url;

  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}

// 通过 Url 下载文件
export function downLoadByURL(url, name) {
  if (!url) {
    return "";
  }

  const link = document.createElement("a");
  link.style.display = "none";
  link.download = name;
  link.href = url;

  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}
