namespace PopularNetLibraries.Autofac.Sample
{
    public class AppConfiguration : IConfiguration
    {
        public string GetOptionAsString(string optionName)
        {
            if (optionName == "EnableLogger")
                return "yes";
            return null;
        }
    }
}
