using SixLabors.ImageSharp.Formats.Gif;

namespace Infrastructure.GifImage;

/// <summary>
/// 动态GIF创建器
/// </summary>
public class AnimatedGifCreator
{
    /// <summary>
    /// 当前帧数
    /// </summary>
    public int FrameCount { get; private set; } = 0;

    /// <summary>
    /// 延迟时间（毫秒）
    /// </summary>
    public uint Delay { get; }

    /// <summary>
    /// 重复次数（0：循环）
    /// </summary>
    public uint Repeat { get; }

    /// <summary>
    /// 获取Gif图片
    /// </summary>
    public Image? Image => _image;
    private Image? _image;

    /// <summary>
    /// 获取Gif图片Base64字符串
    /// </summary>
    public string? Base64 => Image?.ToBase64String(GifFormat.Instance);

    /// <summary>
    /// 动态GIF创建器
    /// </summary>
    /// <param name="delay">延迟时间（毫秒）</param>
    /// <param name="repeat">重复次数（0：循环）</param>
    public AnimatedGifCreator(Image[] images, uint delay = 33, uint repeat = 0)
    {
        Delay = delay;
        Repeat = repeat;
        FrameCount = images.Length;

        if (images?.Length > 0)
        {
            var firstImg = images[0];

            var gifMetaData = firstImg.Metadata.GetGifMetadata();
            gifMetaData.RepeatCount = 0;

            var metaData = firstImg.Frames.RootFrame.Metadata.GetGifMetadata();
            metaData.FrameDelay = (int)delay;

            for (int i = 1; i < images.Length; i++)
            {
                var img = images[i];

                metaData = img.Frames.RootFrame.Metadata.GetGifMetadata();
                metaData.FrameDelay = (int)delay;

                firstImg.Frames.AddFrame(img.Frames.RootFrame);
            }

            _image = firstImg;
        }
    }
}