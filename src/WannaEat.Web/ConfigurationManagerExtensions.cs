namespace WannaEat.Web;

public static class ConfigurationManagerExtensions
{
    public static bool IsHeroku(this ConfigurationManager manager)
    {
        return manager.GetValue<int>("HEROKU_APP") == 1;
    }
}