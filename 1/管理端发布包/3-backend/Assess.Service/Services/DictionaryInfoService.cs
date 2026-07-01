using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IDictionaryInfoService : IBusinessServiceBase<DictionaryInfo>
{
    /// <summary>
    /// 添加新字典信息
    /// </summary>
    /// <param name="key">字典键</param>
    /// <param name="value">字典值</param>
    /// <returns></returns>
    Task<RequestResult> InsertAsync(DictionaryType key, string value);

    /// <summary>
    /// 更改字典信息
    /// </summary>
    /// <param name="id">字典标识</param>
    /// <param name="value">字典值</param>
    /// <returns></returns>
    Task<RequestResult> UpdateAsync(long id, string value);
}

public class DictionaryInfoService : BusinessServiceBase<DictionaryInfo>, IDictionaryInfoService
{
    public DictionaryInfoService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }

    public async Task<RequestResult> InsertAsync(DictionaryType key, string value)
    {
        RequestResult result = new();

        long count = await GetCountAsync(e => e.Key == key && e.Value == value).NotWaitBack();

        if (count > 0) { result.Fail("该字典信息已存在"); }
        else
        {
            DictionaryInfo entity = new() { Key = key, Value = value };
            bool isSucc = await InsertAsync(entity).NotWaitBack();

            if (isSucc) { result.Success("添加成功"); }
            else { result.Fail("添加失败"); }
        }

        return result;
    }

    public async Task<RequestResult> UpdateAsync(long id, string value)
    {
        RequestResult result = new();

        var entity = await GetEntityAsync(id).NotWaitBack();
        if (entity == null) { result.Fail("您要修改的信息不存在"); }
        else
        {
            long count = await GetCountAsync(e => e.Key == entity.Key && e.Value == value).NotWaitBack();
            if (count > 0) { result.Fail("该字典值已存在"); }
            else
            {
                count = await UpdateAsync(e => e.Id == id, e => new DictionaryInfo { Value = value }).NotWaitBack();
                if (count > 0) { result.Success("更改成功"); }
                else { result.Fail("更改失败"); }
            }
        }

        return result;
    }
}
