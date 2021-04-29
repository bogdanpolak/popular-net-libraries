using LanguageExt;

namespace PopularNetLibraries.Autofac.Sample
{
    public interface IConfiguration
    {
        Option<string> GetOptionAsString(string optionName);
    }
}
