namespace Infrastructure.Utilities;

public static class EnumHelper
{
    public static T Random<T>(params T[] excludes)
    {
        Type enumType = typeof(T);
        string[] array = Enum.GetNames(enumType);
        if (excludes.Length > 0)
        {
            IEnumerable<string> second = excludes.Select(s => Enum.GetName(enumType, s!)!);
            array = array.Except(second).ToArray();
        }
        int num = new Random().Next(0, array.Length);
        string value = array[num];
        return (T)Enum.Parse(enumType, value);
    }
}