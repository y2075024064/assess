namespace Infrastructure.Utilities;

public class UserToken
{
    public static string NowToken()
    {
        string guid = Guid.NewGuid().ToString().Replace("-", "");
        var random = new Random(DateTime.Now.Millisecond);
        char[] chars = new char[32];
        for (int i = 0; i < 32; i++)
        {
            int num = random.Next(36);
            if (num < 10) { chars[i] = (char)(num + 48); }
            else { chars[i] = (char)(num + 87); }
        }

        return guid + new string(chars);
    }
}