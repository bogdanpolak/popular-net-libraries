using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using static LanguageExt.Prelude;

namespace PopularNetLibraries.Autofac.Sample
{
    public class AppConfiguration : IConfiguration
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public Option<string> GetOptionAsString(string optionName)
        {
            return optionName.ToLower() switch
            {
                "enablelogger" => "yes",
                _ => None
            };
        }
    }
}
