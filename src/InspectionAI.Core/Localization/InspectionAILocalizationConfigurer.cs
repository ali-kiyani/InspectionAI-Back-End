using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace InspectionAI.Localization
{
    public static class InspectionAILocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(InspectionAIConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(InspectionAILocalizationConfigurer).GetAssembly(),
                        "InspectionAI.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
