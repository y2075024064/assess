namespace Infrastructure.Web;

/// <summary>
/// Http请求工具类
/// </summary>
public class HttpRequestTool
{
    /// <summary>
    /// 发起一个HTTP请求（以GET方式）
    /// </summary>
    /// <param name="url">请求URL</param>
    /// <param name="httpContent">请求内容</param>
    /// <param name="timeout">超时时间</param>
    /// <returns>响应内容</returns>
    public static Task<string> HttpGetAsync(string url, HttpContent? httpContent = null, int timeout = 60000)
    {
        return HttpSendAsync(HttpMethod.Get, url, httpContent, timeout);
    }

    /// <summary>
    /// 发起一个HTTP请求（以POST方式）
    /// </summary>
    /// <param name="url">请求URL</param>
    /// <param name="httpContent">请求内容</param>
    /// <param name="timeout">超时时间</param>
    /// <returns>响应内容</returns>
    public static Task<string> HttpPostAsync(string url, HttpContent? httpContent = null, int timeout = 60000)
    {
        return HttpSendAsync(HttpMethod.Post, url, httpContent, timeout);
    }

    /// <summary>
    /// 发起一个HTTP请求
    /// </summary>
    /// <param name="method"></param>
    /// <param name="url">请求URL</param>
    /// <param name="httpContent">请求内容</param>
    /// <param name="timeout">超时时间</param>
    /// <returns>响应内容</returns>
    public async static Task<string> HttpSendAsync(HttpMethod method, string url, HttpContent? httpContent, int timeout = 60000)
    {
        HttpResponseMessage response = await HttpSendRetrunResponseAsync(method, url, httpContent, timeout);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发起一个HTTP请求
    /// </summary>
    /// <param name="method"></param>
    /// <param name="url">请求URL</param>
    /// <param name="httpContent">请求内容</param>
    /// <param name="timeout">超时时间</param>
    /// <returns>响应内容</returns>
    public async static Task<HttpResponseMessage> HttpSendRetrunResponseAsync(HttpMethod method, string url, HttpContent? httpContent, int timeout = 60000)
    {
        using HttpClient client = new();
        client.Timeout = TimeSpan.FromMilliseconds(timeout);

        HttpRequestMessage request = new(method, url);
        if (httpContent != null) { request.Content = httpContent; }
        return await client.SendAsync(request);
    }

    /// <summary>
    /// 发起一个HTTP请求
    /// </summary>
    /// <param name="request">请求消息</param>
    /// <param name="timeout">超时时间</param>
    /// <returns>响应内容</returns>
    public async static Task<string> HttpSendAsync(HttpRequestMessage request, int timeout = 60000)
    {
        HttpResponseMessage response = await HttpSendRetrunResponseAsync(request, timeout);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发起一个HTTP请求
    /// </summary>
    /// <param name="request">请求消息</param>
    /// <param name="timeout">超时时间</param>
    /// <returns>响应内容</returns>
    public async static Task<HttpResponseMessage> HttpSendRetrunResponseAsync(HttpRequestMessage request, int timeout = 60000)
    {
        using HttpClient client = new();
        client.Timeout = TimeSpan.FromMilliseconds(timeout);
        return await client.SendAsync(request);
    }
}