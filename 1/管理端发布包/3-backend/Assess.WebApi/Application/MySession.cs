using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Infrastructure.Extensions;
using Infrastructure.Utilities;
using System.Globalization;

namespace Assess.WebApi;

/// <summary>
/// 我的自定义Session
/// </summary>
public class MySession
{
    private static string headerName = "X-Token";

    private string sessionId;

    private readonly IDistributedCache? _cache;

    private readonly Dictionary<string, object?> keyValues = new();

    public string SessionId => sessionId;

    public MySession(HttpContext httpContext)
    {
        _cache = httpContext.RequestServices.GetService<IDistributedCache>();
        string? sessionId = null;

        if (httpContext.Request.Headers.TryGetValue(headerName, out StringValues value))
        {
            sessionId = value.ToString();
        }

        if (!string.IsNullOrEmpty(sessionId))
        {
            keyValues = GetCache(_cache, sessionId);
            this.sessionId = sessionId;
        }
        else
        {
            this.sessionId = UserToken.NowToken();
            keyValues = GetCache(_cache, this.sessionId);
        }
    }

    /// <summary>
    /// 获取缓存信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">缓存值</param>
    /// <returns></returns>
    public T? Get<T>(string key) where T : struct
    {
        object? value = Get(key);
        if (value == null) { return null; }
        else { return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// 获取缓存信息
    /// </summary>
    /// <param name="key">缓存值</param>
    /// <returns></returns>
    public string? GetString(string key)
    {
        return Get(key)?.ToString();
    }

    /// <summary>
    /// 获取缓存信息
    /// </summary>
    /// <param name="key">缓存值</param>
    /// <returns></returns>
    public object? Get(string key)
    {
        if (keyValues.TryGetValue(key, out object? value))
        {
            _cache?.Refresh(sessionId);
            return value;
        }
        else { return null; }
    }

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key">缓存</param>
    /// <param name="value">缓存值</param>
    public void Set(string key, object? value)
    {
        if (keyValues.ContainsKey(key))
        { keyValues[key] = value; }
        else { keyValues.Add(key, value); }

        SetCache(_cache, sessionId, keyValues);
    }

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="info">缓存信息</param>
    public void Set(Dictionary<string, object?> info)
    {
        foreach (var item in info)
        {
            if (keyValues.ContainsKey(item.Key))
            { keyValues[item.Key] = item.Value; }
            else { keyValues.Add(item.Key, item.Value); }
        }

        SetCache(_cache, sessionId, keyValues);
    }

    /// <summary>
    /// 移除缓存
    /// </summary>
    /// <param name="key">缓存键</param>
    public void Remove(string key)
    {
        if (keyValues.Remove(key))
        {
            SetCache(_cache, sessionId, keyValues);
        }
    }

    /// <summary>
    /// 清理当前会话缓存
    /// </summary>
    /// <param name="sessionId">新的 SessionId</param>
    public void Clear(string? sessionId = null)
    {
        _cache?.Remove(this.sessionId);
        if (!string.IsNullOrWhiteSpace(sessionId))
        {
            this.sessionId = sessionId;
        }
    }

    /// <summary>
    /// 设置 HttpHeaders 名称
    /// </summary>
    /// <param name="name">header名称</param>
    public static void SetHeaderName(string name)
    {
        headerName = name;
    }

    /// <summary>
    /// 从缓存中获取信息
    /// </summary>
    /// <param name="cache">缓存服务</param>
    /// <param name="key">存储键</param>
    /// <returns></returns>
    private static Dictionary<string, object?> GetCache(IDistributedCache? cache, string key)
    {
        string? allValue = cache?.GetString(key);
        if (allValue.IsEmpty()) { return new Dictionary<string, object?>(); }
        else
        {
            var result = allValue.ToModel<Dictionary<string, object?>>();
            return result ?? new Dictionary<string, object?>();
        }
    }

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="cache">缓存服务</param>
    /// <param name="key">存储键</param>
    /// <param name="info">缓存的内容</param>
    private static void SetCache(IDistributedCache? cache, string key, Dictionary<string, object?> info)
    {
        if (cache == null) { return; }
        string json = info.ToJson();
        cache.SetString(key, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(AppSettings.Settings.TokenDuration)
        });
    }
}