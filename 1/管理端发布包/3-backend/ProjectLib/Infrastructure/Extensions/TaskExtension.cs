using System.Runtime.CompilerServices;

namespace Infrastructure.Extensions;

/// <summary>
/// Task 扩展类
/// </summary>
public static class TaskExtension
{
    /// <summary>
    /// 不排队回到原始上下文（Task.ConfigureAwait(false) 方法的扩展用法）
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public static ConfiguredTaskAwaitable NotWaitBack(this Task task)
    {
        return task.ConfigureAwait(false);
    }

    /// <summary>
    /// 不排队回到原始上下文（Task<TResult>.ConfigureAwait(false) 方法的扩展用法）
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    public static ConfiguredTaskAwaitable<TResult> NotWaitBack<TResult>(this Task<TResult> task)
    {
        return task.ConfigureAwait(false);
    }
}