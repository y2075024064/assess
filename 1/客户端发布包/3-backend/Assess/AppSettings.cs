using System.Reflection;

namespace Assess;

/// <summary>
/// 应用程序设置信息模型
/// </summary>
public class AppSettings
{
    public static AppSettings Settings { get; private set; } = new AppSettings();

    public string MchId { get; set; } = string.Empty;

    public string AppID { get; set; } = string.Empty;

    public string AppSecret { get; set; } = string.Empty;

    public string APIKey { get; set; } = string.Empty;

    public string CertSerialNo { get; set; } = string.Empty;

    public string MchCertPem { get; set; } = string.Empty;

    public string MchKeyPem { get; set; } = string.Empty;

    public string WxCertPem { get; set; } = string.Empty;

    public string NotifyUrl { get; set; } = string.Empty;

    public uint TokenDuration { get; set; }

    public uint VerCodeDuration { get; set; }

    public string UploadAddress { get; set; } = string.Empty;

    public string UploadDirectory { get; set; } = string.Empty;

    public string MiniprogramPage { get; set; } = string.Empty;

    public string MiniprogramState { get; set; } = string.Empty;

    public static void LoadConfig(Func<string, string?> func)
    {
        Settings = new();
        Type type = Settings.GetType();
        PropertyInfo[] ps = type.GetProperties();
        foreach (PropertyInfo prop in ps)
        {
            if (prop.GetMethod?.IsStatic == true) { continue; }

            var value = func(prop.Name);
            if (value != null)
            {
                object obj = Convert.ChangeType(value, prop.PropertyType);
                prop.SetValue(Settings, obj, null);
            }
        }
    }
}