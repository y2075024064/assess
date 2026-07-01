using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace Infrastructure.VerCodeImage;

/// <summary>
/// 作    用：创建验证码图片(字符组型)
/// 作    者：官伟
/// </summary>
public class CharGroup
{
    /// <summary>
    /// 图片验证码
    /// 验证码长度（默认：4）
    /// 图片宽度（默认：80px）
    /// 图片高度（默认：40px）
    /// </summary>
    public CharGroup()
    {
    }

    /// <summary>
    /// 图片验证码
    /// </summary>
    /// <param name="length">验证码长度</param>
    public CharGroup(int length)
    {
        this.length = length;
    }

    /// <summary>
    /// 图片验证码
    /// </summary>
    /// <param name="length">验证码长度</param>
    /// <param name="width">图片宽度（px）</param>
    /// <param name="height">图片高度（px）</param>
    public CharGroup(int length, int width, int height) : this(length)
    {
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// 设置或获取验证码的长度
    /// 默认：4
    /// </summary>
    public int Length
    {
        get => length;
        set
        {
            if (value <= 0) { throw new Exception("验证码的长度Length不能小于等于0"); }
            length = value;
        }
    }
    private int length = 4;

    /// <summary>
    /// 设置或获取验证码的宽
    /// 默认：80
    /// </summary>
    public int Width
    {
        get => width;
        set => width = value;
    }
    private int width = 80;

    /// <summary>
    /// 设置或获取验证码的高
    /// 默认：40
    /// </summary>
    public int Height
    {
        get => height;
        set => height = value;
    }
    private int height = 40;

    /// <summary>
    /// 设置验证码是否画干扰线
    /// 默认：false
    /// </summary>
    public bool InterferingLine { set => interLine = value; }
    private bool interLine = false;

    /// <summary>
    /// 设置验证码是否画前景干扰点
    /// 默认：false
    /// </summary>
    public bool InterferingSpot { set => interSpot = value; }
    private bool interSpot = false;

    /// <summary>
    /// 设置验证码的背景颜色
    /// 默认：白色
    /// </summary>
    public Color BackColor { set => backColor = value; }
    private Color backColor = Color.White;

    /// <summary>
    /// 设置验证码字体集
    /// 默认：Arial, 华文行楷, hakuyoxingshu7000
    /// </summary>
    public string[] ArryFontFamily { set => arryFontFamily = value; }
    private string[] arryFontFamily = { "Arial", "Georgia", "Verdana", "Comic Sans MS", "Microsoft Sans Serif" };

    /// <summary>
    /// 设置验证码的颜色集
    /// 默认：Blue、Green、Red、Orange、Violet、Black
    /// </summary>
    public Color[] ArryColor { set => arryColor = value; }
    private Color[] arryColor = { Color.Blue, Color.Green, Color.Red, Color.Orange, Color.Violet, Color.Black };

    /// <summary>
    /// 获取验证码
    /// </summary>
    public string ValidateStr => validateStr;
    private string validateStr = "";

    /// <summary>
    /// 生成验证码
    /// </summary>
    protected void CreateVercode()
    {
        //验证码源
        string str = "23456789ABCDEFGHJKLMNPRSTUVWXYabdefghijmnrtxy";

        //以时间为种子生成随机生成器
        int seekSeek = unchecked((int)DateTime.Now.Ticks);
        Random seekRand = new(seekSeek);

        //验证码随机序列
        int[] randomNums = new int[Length];

        //生成验证码随机序列
        for (int i = 0; i < Length; i++)
            randomNums[i] = seekRand.Next(0, str.Length);

        //根据随机序列生成验证码
        validateStr = "";
        for (int i = 0; i < Length; i++)
            validateStr += str[randomNums[i]];
    }

    /// <summary>
    /// 创建透明背景的干扰图片
    /// </summary>
    /// <returns></returns>
    protected Image CreateDisturbImg()
    {
        //以时间为种子生成随机生成器
        Random random = new(DateTime.Now.Millisecond);

        //创建图片
        var image = new Image<Argb32>(width, height, Color.Transparent);

        //画干扰线
        if (interLine)
        {
            for (int i = 0; i < 7; i++)
            {
                int x2, x1 = random.Next(width);
                int y2, y1 = random.Next(height);

                do { x2 = random.Next(width); }
                while (x1 == x2);

                do { y2 = random.Next(height); }
                while (y1 == y2);

                var color = arryColor[random.Next(arryColor.Length)];
                image.Mutate(draw =>
                    draw.DrawLine(color, 1, new[] { new PointF(x1, y1), new PointF(x2, y2) })
                );
            }
        }

        //画图片的前景干扰点
        if (interSpot)
        {
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(width);
                int y = random.Next(height);
                byte r = (byte)random.Next(256);
                byte g = (byte)random.Next(256);
                byte b = (byte)random.Next(256);

                image[x, y] = Color.FromRgb(r, g, b);
            }
        }

        return image;
    }

    /// <summary>
    /// 创建单张验证码图片
    /// </summary>
    public Image CreateSingleVercodeImg()
    {
        CreateVercode();

        //以时间为种子生成随机生成器
        Random random = new(DateTime.Now.Millisecond);

        //创建图片
        var image = new Image<Argb32>(width, height, backColor);

        //计算绘制数据
        float fontSize = height / 1.4f;
        float boxWidth = width / length;
        float halfBoxW = boxWidth / 2f;
        float halfH = height / 2f - 1;
        float r_h = height - 1;
        int colorsCount = arryColor.Length;

        FontCollection fonts = new FontCollection().AddSystemFonts();
        for (int i = 0; i < length; i++)
        {
            //获取字体
            string fontFamilyName = arryFontFamily[random.Next(arryFontFamily.Length)];
            if (!fonts.TryGet(fontFamilyName, out FontFamily fontFamily))
            {
                int count = fonts.Families.Count();
                fontFamily = fonts.Families.ElementAt(random.Next(count));
            }

            float x = i * boxWidth;

            //创建文本绘制样式
            float t_x = x - 1;
            Font font = new(fontFamily, fontSize, FontStyle.Bold);
            RichTextOptions richTextOptions = new(font)
            {
                Origin = new PointF(t_x + halfBoxW, halfH),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            //获取渐变颜色索引
            int index2, index1 = random.Next(colorsCount);
            do { index2 = random.Next(colorsCount); }
            while (index1 == index2);

            //创建渐变画笔
            float b_x = x == 0 ? 0 : x - 1;
            Brush brush = new LinearGradientBrush(
                new PointF(b_x, 0),
                new PointF(b_x + boxWidth, r_h),
                GradientRepetitionMode.None,
                new ColorStop[]
                {
                    new ColorStop(0, arryColor[index1]),
                    new ColorStop(1, arryColor[index2])
                }
            );

            string text = validateStr![i].ToString();
            image.Mutate(draw => draw.DrawText(richTextOptions, text, brush));
        }

        //添加前景干扰
        var disturbImg = CreateDisturbImg();
        image.Mutate(draw => draw.DrawImage(disturbImg, 1));

        return image;
    }

    /// <summary>
    /// 创建多张验证码图片
    /// </summary>
    public Image[] CreateVercodeImgs()
    {
        CreateVercode();

        //以时间为种子生成随机生成器
        Random random = new(DateTime.Now.Millisecond);

        //生成图片
        Image[] images = new Image[length];
        for (int i = 0; i < length; i++)
        {
            images[i] = new Image<Argb32>(width, height, backColor);
        }

        //生成字体样式信息
        List<Font> fontList = new();
        float fontSize = height / 1.4f;
        int colorsCount = arryColor.Length;
        FontCollection fonts = new FontCollection().AddSystemFonts();
        for (int i = 0; i < length; i++)
        {
            string fontFamilyName = arryFontFamily[random.Next(arryFontFamily.Length)];
            if (!fonts.TryGet(fontFamilyName, out FontFamily fontFamily))
            {
                int count = fonts.Families.Count();
                fontFamily = fonts.Families.ElementAt(random.Next(count));
            }
            fontList.Add(new(fontFamily, fontSize, FontStyle.Bold));
        }

        //计算绘制数据
        float boxWidth = width / length;
        float halfBoxW = boxWidth / 2f;
        float halfH = height / 2f - 1;
        float r_h = height - 1;

        //写入验证码
        for (int i = 0; i < length; i++)
        {
            float x = i * boxWidth;

            //创建文本绘制样式
            float t_x = x - 1;
            RichTextOptions richTextOptions = new(fontList[i])
            {
                Origin = new PointF(t_x + halfBoxW, halfH),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            //获取渐变颜色索引
            int index2, index1 = random.Next(colorsCount);
            do { index2 = random.Next(colorsCount); }
            while (index1 == index2);

            //创建渐变笔刷
            float b_x = x == 0 ? 0 : x - 1;
            Brush brush = new LinearGradientBrush(
                new PointF(b_x, 0),
                new PointF(b_x + boxWidth, r_h),
                GradientRepetitionMode.None,
                new ColorStop[]
                {
                    new ColorStop(0, arryColor[index1]),
                    new ColorStop(1, arryColor[index2])
                }
            );

            string text = validateStr![i].ToString();
            for (int j = 0; j < length; j++)
            {
                if (i == j) { continue; }
                images[j].Mutate(draw => draw.DrawText(richTextOptions, text, brush));
            }
        }

        //添加前景干扰
        var disturbImg = CreateDisturbImg();
        for (int i = 0; i < length; i++)
        {
            images[i].Mutate(draw => draw.DrawImage(disturbImg, 1));
        }

        return images;
    }
}
