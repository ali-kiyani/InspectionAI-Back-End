using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace InspectionAI
{
    public class UserSettingsProvider : SettingProvider
    {
        public const string DefaultRefreshIntervalPhrase = "Abp.Localization.RefreshInterval";
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                    new SettingDefinition(
                        DefaultRefreshIntervalPhrase,
                        "1",
                        scopes: SettingScopes.User,
                        isVisibleToClients: true
                        )
            };
        }

    }
}
