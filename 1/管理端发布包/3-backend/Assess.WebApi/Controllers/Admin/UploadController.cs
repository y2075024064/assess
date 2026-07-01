using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 上传文件
/// </summary>
public class UploadController : AdminApiController<Member, IMemberService>
{
    public UploadController(IMemberService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="type">图片类型</param>
    /// <returns></returns>
    [HttpPost]
    public Task<RequestStrResult> Image(ImageType type)
    {
        return Task.Run(() =>
        {
            RequestStrResult result = new();

            if (Request.ContentType?.Contains("json") == true)
            {
                var reader = new StreamReader(Request.Body);
                string json = reader.ReadToEnd();
                var obj = json.ToModel() as dynamic;

                if (obj != null)
                {
                    string base64 = obj.data;

                    string name = WebFileTool.SaveImage(base64, type);
                    result.Data = WebFileTool.GetImageUrl(name, type);
                    result.Success("上传成功");
                }
            }
            else
            {
                var files = Request.Form.Files;
                if (files?.Count > 0)
                {
                    IFormFile file = files[0];
                    if (file.Length > 3145728) { result.Fail("图片不得超过3M"); }
                    else if (!file.ContentType.StartsWith("image"))
                    { result.Fail("上传的文件必须是图片"); }
                    else
                    {
                        string name = WebFileTool.SaveImage(file, type);
                        result.Data = WebFileTool.GetImageUrl(name, type);
                        result.Success("上传成功");
                    }
                }
                else
                {
                    result.Fail("您未上传任何文件");
                }
            }

            return result;
        });
    }

    /// <summary>
    /// 上传视频
    /// </summary>
    /// <param name="type">视频类型</param>
    /// <returns></returns>
    [HttpPost]
    public Task<RequestStrResult> Video(VideoType type)
    {
        return Task.Run(() =>
        {
            RequestStrResult result = new();

            var files = Request.Form.Files;
            if (files?.Count > 0)
            {
                IFormFile file = files[0];

                if (file.Length > 1610612736)
                {
                    // （720p，H.264编码
                    result.Fail("视频不得超过1.5GB");
                }

                // 检查文件类型
                else if (!file.ContentType.StartsWith("video"))
                {
                    result.Fail("上传的文件必须是视频");
                }
                else
                {
                    try
                    {
                        // 使用 WebFileTool 保存视频文件
                        string fileName = WebFileTool.SaveVideoFile(file, type);

                        // 返回视频访问 URL
                        result.Data = WebFileTool.GetVideoUrl(fileName, type);
                        result.Success("上传成功");
                    }
                    catch (Exception ex)
                    {
                        result.Fail($"上传失败: {ex.Message}");
                    }
                }
            }
            else
            {
                result.Fail("您未上传任何文件");
            }

            return result;
        });
    }

    /// <summary>
    /// 删除图片
    /// </summary>
    /// <param name="imgs">图片集合</param>
    /// <param name="type">图片类型</param>
    /// <returns></returns>
    [HttpPost]
    public Task<RequestResult> DeleteImg([FromBody] string[] imgs, [FromQuery] ImageType type)
    {
        return Task.Run(() =>
        {
            RequestResult result = new();

            if (imgs?.Length > 0)
            {
                string preUrl = AppSettings.Settings.UploadAddress;
                foreach (string img in imgs)
                {
                    if (img.StartsWith(preUrl))
                    {
                        string name = Path.GetFileName(img);
                        WebFileTool.DeleteImage(name, type);
                    }
                }
            }

            result.Success();

            return result;
        });
    }


    /// <summary>
    /// 删除视频
    /// </summary>
    /// <param name="fileName">要删除的视频文件名</param>
    /// <param name="type">视频类型</param>
    /// <returns></returns>
    [HttpDelete]
    public Task<RequestResult> DeleteVideo(string fileName, VideoType type)
    {
        return Task.Run(() =>
        {
            RequestResult result = new();

            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    result.Fail("未提供有效的文件名");
                    return result;
                }

                // 使用 WebFileTool 删除视频文件
                WebFileTool.DeleteVideoFile(fileName, type);

                result.Success("删除成功");
            }
            catch (Exception ex)
            {
                result.Fail($"删除失败: {ex.Message}");
            }

            return result;
        });
    }
}
