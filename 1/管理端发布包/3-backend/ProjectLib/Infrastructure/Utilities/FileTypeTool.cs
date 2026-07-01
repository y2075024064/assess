using Infrastructure.Extensions;

namespace Infrastructure.Utilities;

/// <summary>
/// 文件类型工具类
/// </summary>
public static class FileTypeTool
{
    /// <summary>
    /// 判断文件是否为 电子表格文件（.xlsx）
    /// </summary>
    /// <param name="stream">文件流对象</param>
    /// <param name="name">文件名称</param>
    /// <returns></returns>
    public static bool IsExcelWithXlsx(this Stream stream, string name)
    {
        if (name.IsEmpty()) { return false; }

        string _name = name.ToLower();
        if (!_name.EndsWith(".xlsx")) { return false; }

        byte[] sign = new byte[8];
        stream.Read(sign, 0, 8);

        int[] _sign = new[] { 80, 75, 03, 04, -1, -1, 0, 0 };
        for (int i = 0; i < 8; i++)
        {
            int item = _sign[i];
            if (item < 0) { continue; }
            if (sign[i] != item) { return false; }
        }

        return true;
    }
}
