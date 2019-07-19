using System;
using BasicAppSettings;

namespace Sample
{
    /// <summary>
    /// Sets application settings using the defaults or the values specified in the configuration file.
    /// </summary>
    internal static class AppConfig
    {
        internal static string Operations =
            Config.GetValue<string>("Operations", "Create|Read|Update|Delete|Assign|Revoke|Enable|Disable");

        internal static string Objects =
            Config.GetValue<string>("Objects", "User|Group|Role");

        internal static char Code =
            Config.GetValue<char>("Code", 'C');

        internal static char Index =
            Config.GetValue<char>("Index", 'X');

        internal static bool Enabled =
            Config.GetValue<bool>("Enabled", true);

        internal static bool Enforce =
            Config.GetValue<bool>("Enforce", true);

        internal static int Max =
            Config.GetValue<int>("Max", 1000);

        internal static int Min =
            Config.GetValue<int>("Min", 5);

        internal static DateTime FirstDate =
            Config.GetValue<DateTime>("FirstDate", DateTime.UtcNow);

        internal static DateTime LastDate =
            Config.GetValue<DateTime>("LastDate", DateTime.UtcNow);
    }
}
