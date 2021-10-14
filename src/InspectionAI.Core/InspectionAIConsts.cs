using InspectionAI.Debugging;

namespace InspectionAI
{
    public class InspectionAIConsts
    {
        public const string LocalizationSourceName = "InspectionAI";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "3f71beb6d31143bdabfe40b369f9a409";
    }
}
