using System.Reflection;

namespace Assess.WebApi;

/// <summary>
/// 字段对照交换值
/// </summary>
public static class FieldChange
{
    /// <summary>
    /// 交换字段值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model">目标对象</param>
    /// <param name="param">参数对象</param>
    /// <param name="ignores">忽略交换的字段名称集合</param>
    /// <returns></returns>
    public static bool ChangeField<T>(this T model, object param, params string[] ignores) where T : class
    {
        bool hasChange = false;
        Type model_type = model.GetType();
        List<string> igList = ignores?.ToList() ?? new();
        PropertyInfo[] param_ps = param.GetType().GetProperties();

        foreach (PropertyInfo param_p in param_ps)
        {
            if (igList.Contains(param_p.Name) || !param_p.CanRead) { continue; }

            PropertyInfo? model_p = model_type.GetProperty(param_p.Name);
            if (model_p?.CanWrite == true &&
               (param_p.PropertyType.IsAssignableTo(model_p.PropertyType) ||
                param_p.PropertyType.IsAssignableFrom(model_p.PropertyType)))
            {
                object? model_value = model_p.GetValue(model, null);
                object? param_value = param_p.GetValue(param, null);

                if (param_value != null && param_value.Equals(model_value)) { continue; }
                if (param_value == null && model_value == null) { continue; }
                model_p.SetValue(model, param_value, null);
                hasChange = true;
            }
        }

        return hasChange;
    }

    /// <summary>
    /// 交换字段值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model">目标对象</param>
    /// <param name="fieldName">字段名称</param>
    /// <param name="value">目标值</param>
    /// <returns></returns>
    public static bool ChangeField<T>(this T model, string fieldName, string? value) where T : class
    {
        bool hasChange = false;
        Type model_type = model.GetType();

        PropertyInfo? model_p = model_type.GetProperty(fieldName);
        if (model_p?.CanWrite == true)
        {
            Type type = model_p.PropertyType;
            if (type.IsGenericType) { type = type.GenericTypeArguments[0]; }

            object? val = value;
            if (type.Name != "String")
            {
                val = Convert.ChangeType(value, type);
            }

            model_p.SetValue(model, val, null);
            hasChange = true;
        }

        return hasChange;
    }
}
