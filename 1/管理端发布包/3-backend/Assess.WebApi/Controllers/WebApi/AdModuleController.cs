using System.ComponentModel.DataAnnotations;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.WebApi.Controllers.WebApi
{
    /// <summary>
    /// 广告信息相关接口
    /// </summary>
    public class AdModuleController : WebApiController<AdModule, IAdModuleService>
    {
        private readonly IAdvertisingService _advertisingService;

        public AdModuleController(IAdModuleService entityService,
            IAdvertisingService advertisingService) : base(entityService)
        {
            _advertisingService = advertisingService;
        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestResult<ModuleInfoModel>> GetAdInfo(
            [Required(ErrorMessage = "模块名称必传")] string moduleName)
        {
            RequestResult<ModuleInfoModel> result = new();

            var entity = await entityService.GetEntityAsync(e => e.ModuleName == moduleName).NotWaitBack();

            if (entity == null) { result.Fail("不存在该广告模块"); }
            else
            {
                List<Advertising> ads = await _advertisingService.GetListAsync(e => e.ModuleId == entity.Id,
                    e => new Advertising
                    {
                        Id = e.Id,
                        Type = e.Type,
                        Title = e.Title,
                        FolderUrl = e.FolderUrl,
                        Hyperlink = e.Hyperlink,
                        PreviewImg = e.PreviewImg,
                    }).NotWaitBack();

                if (ads.Count <= 0) { result.Fail("该广告模块没有广告"); }
                else
                {
                    result.Data = new ModuleInfoModel()
                    {
                        EnableMark = entity.IsEnable,
                        IsCarousel = entity.IsCarousel,
                        AdList = ads.Select(e =>
                        {
                            AdInfoModel model = new()
                            {
                                Id = e.Id,
                                Type = e.Type,
                                Title = e.Title,
                                PreviewImg = WebFileTool.GetImageUrl(e.PreviewImg, ImageType.Preview),
                            };
                            switch (e.Type)
                            {
                                case AdvertisingType.富文本: model.Hyperlink = null; break;
                                case AdvertisingType.外链接: model.Hyperlink = e.Hyperlink; break;
                                case AdvertisingType.Html文件:
                                    model.Hyperlink = WebFileTool.GetHtmlFolderUrl(e.FolderUrl);
                                    break;
                            }

                            return model;
                        }).ToArray(),
                    };
                    result.Success();
                }
            }

            return result;
        }


        /// <summary>
        /// 获取广告富文本内容
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestStrResult> AdContent(
            [Required(ErrorMessage = "模块名称必传")] int id)
        {
            RequestStrResult result = new();

            var ad_entity = await _advertisingService.GetEntityAsync(e => e.Id == id, e =>
                new Advertising { Content = e.Content }).NotWaitBack();

            if (ad_entity == null) { result.Fail("您要请求的数据不存在"); }
            else
            {
                result.Data = ad_entity.Content;
                result.Success();
            }

            return result;
        }
    }
}
