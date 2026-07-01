using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using Infrastructure.Extensions;
using SixLabors.ImageSharp;
using Infrastructure.Web;

namespace Assess.WebApi;

/// <summary>
/// 文件工具类
/// </summary>
public class WebFileTool
{
    private static string _uploadAddress = string.Empty;

    private static string _uploadDirectory = string.Empty;

    private static readonly string video = "videos/";
    private static readonly string image = "images/";
    private static readonly string avatar = "avatars/";

    /// <summary>
    /// 初始化工具类
    /// </summary>
    /// <param name="webRootPath"></param>
    /// <param name="uploadAddress"></param>
    /// <param name="uploadDirectory"></param>
    public static void Init(string webRootPath, string uploadAddress, string uploadDirectory)
    {
        _uploadAddress = uploadAddress;
        _uploadDirectory = webRootPath + uploadDirectory;
    }

    /// <summary>
    /// 保存头像图片
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string SaveAvatar(IFormFile file)
    {
        return SaveFile(file, $"{image}{avatar}");
    }

    /// <summary>
    /// 保存图片
    /// </summary>
    /// <param name="file"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string SaveImage(IFormFile file, ImageType? type = null)
    {
        if (type == null) { return SaveFile(file, image); }
        else { return SaveFile(file, $"{image}{type}/"); }
    }

    /// <summary>
    /// 保存图片
    /// </summary>
    /// <param name="base64"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string SaveImage(string base64, ImageType? type = null)
    {
        if (type == null) { return SaveFile(base64, image); }
        else { return SaveFile(base64, $"{image}{type}/"); }
    }

    /// <summary>
    /// 保存视频文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string SaveVideoFile(IFormFile file, VideoType? type = null)
    {
        if (type == null) { return SaveFile(file, video); }
        else { return SaveFile(file, $"{video}{type}/"); }
    }

    /// <summary>
    /// 删除图片资源
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public static void DeleteImage(string? fileName, ImageType? type = null)
    {
        if (type == null) { DeleteFile(fileName, image); }
        else { DeleteFile(fileName, $"{image}{type}/"); }
    }

    /// <summary>
    /// 删除头像图片
    /// </summary>
    /// <param name="fileName"></param>
    public static void DeleteAvatar(string? fileName)
    {
        DeleteFile(fileName, $"{image}{avatar}");
    }

    /// <summary>
    /// 删除商品图标
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public static void DeleteVideoFile(string? fileName, VideoType? type = null)
    {
        if (type == null) { DeleteFile(fileName, video); }
        else { DeleteFile(fileName, $"{video}{type}/"); }
    }

    /// <summary>
    /// 获取头像网址
    /// </summary>
    /// <param name="fileName">头像名称</param>
    /// <returns></returns>
    public static string GetAvatarUrl(string? fileName)
    {
        return GetResourceUrl(fileName, $"{image}{avatar}");
    }

    /// <summary>
    /// 获取图片网址
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetImageUrl(string? fileName, ImageType type)
    {
        return GetResourceUrl(fileName, $"{image}{type}/");
    }

    /// <summary>
    /// 获取视频网址
    /// </summary>
    /// <param name="fileName">视频名称</param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetVideoUrl(string? fileName, VideoType? type = null)
    {
        if (type == null) { return GetResourceUrl(fileName, video); }
        else { return GetResourceUrl(fileName, $"{video}{type}/"); }
    }

    /// <summary>
    /// 获取 Html 文件网址
    /// </summary>
    /// <param name="htmlPath">html相对路径</param>
    /// <returns></returns>
    public static string GetHtmlFolderUrl(string? htmlPath)
    {
        if (!string.IsNullOrEmpty(htmlPath))
        { return $"{_uploadAddress}htmls/{htmlPath}"; }
        return "";
    }

    /// <summary>
    /// 保存 Html 文件夹
    /// </summary>
    /// <param name="files"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static string? SaveHtmlFolder(IFormFile[] files, out string? msg)
    {
        msg = null;

        int count = 0;
        string indexName = "";
        foreach (IFormFile file in files)
        {
            string name = file.Name.ToLower();
            if (name == "index.html")
            { ++count; indexName = file.Name; }
        }

        if (count != 1)
        {
            if (count == 0) { msg = "您上传的文件中必须包含 index.html 入口文件"; }
            else { msg = "您上传的文件中有且只能含有一个 index.html 入口文件"; }
        }
        else
        {
            string path = Guid.NewGuid().ToString().Replace("-", "");
            string basePath = $"{_uploadDirectory}htmls/{path}/";

            foreach (IFormFile file in files)
            {
                string filePath = $"{basePath}{file.Name}";
                using Stream stream = file.OpenReadStream();
                using FileStream fileStream = File.Create(filePath);
                stream.CopyTo(fileStream);
            }

            return $"{path}/{indexName}";
        }

        return null;
    }

    /// <summary>
    /// 删除 Html 文件夹
    /// </summary>
    /// <param name="htmlPath">Html 文件相对路径</param>
    public static void DeleteHtmlFolder(string? htmlPath)
    {
        if (string.IsNullOrEmpty(htmlPath)) { return; }
        string path = $"{_uploadDirectory}htmls/{htmlPath}";
        string? directory = Path.GetDirectoryName(path);
        if (Directory.Exists(directory)) { Directory.Delete(directory, true); }
    }

    /// <summary>
    /// 获取图片资源网址
    /// </summary>
    /// <param name="path">图片名称</param>
    /// <param name="name">资源文件夹名称</param>
    /// <returns></returns>
    private static string GetResourceUrl(string? path, string name)
    {
        if (string.IsNullOrWhiteSpace(path)) { return ""; }
        return $"{_uploadAddress}{name}{path}";
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string SaveFile(IFormFile file, string name)
    {
        string fileDir = $"{_uploadDirectory}{name}";
        string? extension = Path.GetExtension(file.FileName);
        string fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}{extension}";
        if (!Directory.Exists(fileDir)) { _ = Directory.CreateDirectory(fileDir); }

        string filePath = $"{fileDir}{fileName}";
        using Stream stream = file.OpenReadStream();
        using FileStream fileStream = File.Create(filePath);
        stream.CopyTo(fileStream);

        return fileName;
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="base64"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string SaveFile(string base64, string name)
    {
        int index1 = base64.IndexOf('/');
        int index2 = base64.IndexOf(';');
        byte[] data = Convert.FromBase64String(base64[(index2 + 8)..]);
        string extension = base64.Substring(index1 + 1, index2 - index1 - 1);

        string fileName = Guid.NewGuid().ToString() + "." + extension;
        string fileDir = $"{_uploadDirectory}{name}";
        if (!Directory.Exists(fileDir)) { _ = Directory.CreateDirectory(fileDir); }

        string filePath = $"{fileDir}{fileName}";
        using Stream stream = new MemoryStream(data);
        using FileStream fileStream = File.Create(filePath);
        stream.CopyTo(fileStream);

        return fileName;
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="name"></param>
    private static void DeleteFile(string? fileName, string name)
    {
        if (string.IsNullOrWhiteSpace(fileName)) { return; }
        string filePath = $"{_uploadDirectory}{name}{fileName}";
        if (File.Exists(filePath)) { File.Delete(filePath); }
    }

    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="name">图片名称</param>
    /// <param name="width">缩略图长度</param>
    /// <param name="height">缩略图宽度</param>
    /// <param name="type">图片类型</param>
    public static void CreateThumbnail(string name, int width, int height, ImageType? type = null)
    {
        string path;
        if (type == null) { path = $"{_uploadDirectory}{image}{name}"; }
        else { path = $"{_uploadDirectory}{image}{type}/{name}"; }

        int index = path.LastIndexOf(".");
        string fileExt = path[index..];
        string newPath = path.Replace(fileExt, ".thumb.png");

        using Image imageFrom = Image.Load(path);

        // 源图宽度及高度
        int imgFromW = imageFrom.Width;
        int imgFromH = imageFrom.Height;

        IImageEncoder encoder = new PngEncoder();
        if (imgFromW <= width && imgFromH <= height)
        {
            // 如果源图的大小没有超过缩略图指定的大小，则直接把源图复制到目标文件
            imageFrom.Save(newPath, encoder);
            return;
        }

        float scale = height / (float)imgFromH;
        float _scale = width / (float)imgFromW;
        if (_scale < scale) { scale = _scale; }

        width = (int)(imgFromW * scale);
        height = (int)(imgFromH * scale);
        imageFrom.Mutate(x => x.Resize(width, height));
        imageFrom.Save(newPath, encoder);
    }

    /// <summary>
    /// 保存微信头像
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<string?> SaveWechatAvatarAsync(string url)
    {
        var res = await HttpRequestTool.HttpSendRetrunResponseAsync(HttpMethod.Get, url, null).NotWaitBack();
        if (res != null)
        {
            var stream = await res.Content.ReadAsStreamAsync().NotWaitBack();

            string fileName = Guid.NewGuid().ToString() + ".png";
            Image img = Image.Load(stream);

            string fileDir = $"{_uploadDirectory}{image}{avatar}";
            if (!Directory.Exists(fileDir)) { _ = Directory.CreateDirectory(fileDir); }

            string filePath = $"{fileDir}{fileName}";
            await img.SaveAsPngAsync(filePath).NotWaitBack();

            return fileName;
        }

        return null;
    }
}
