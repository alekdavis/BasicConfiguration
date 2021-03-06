# BasicConfiguration
This .NET project illustrates how to keep your application configuration files nice and clean. It also suggests a better way to initialize sensitive and non-sensitive application settings.

## Introduction
We often use configuration (`app.config` or `web.config`) files to store .NET application settings that are not likely to change. As a result, the configuration files get more bloated than they need to be. There must be a better way. And there is.

## Overview
Here is the basic idea:

- Keep the application settings that are not likely to change in the application source code, BUT
- In the unlikely case that they do change before the application can be rebuilt, allow the application to read them from the configuration file.

This project illustrates how this can be done. It also shows how you can handle sensitive application settings, such as passwords, encryption keys, and so on.

IMPORTANT: NEVER KEEP SENSITIVE APPLICATION SETTINGS, SUCH AS PASSWORDS, ENCRIPTION KEYS, CONNECTION STRINGS, AND SO ON, HARD CODED IN THE APPLICATION SOURCE.

## Code
This solution includes two projects:

- [**BasicAppSettings**](../../tree/master/BasicAppSettings) is a [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) class library that implements a couple of helper methods for initializing sensitive and non-sensitive application settings.
- [**SampleConsoleApp**](../../tree/master/SampleConsoleApp) illustrates how to use the `BasicAppSettings` class library.

### BasicAppSettings project
This project implements the class library that exposes a single static class (`Config`) with just a couple of methods:

- `GetValue` - returns either the configuration setting from the application configuration file or (if the setting is missing or unspecified) the default.
- `GetArray` - returns either the configuration setting from the application configuration file or (if the setting is missing or unspecified) the default converted to a string array.
- `GetDictionary` - returns either the configuration setting from the application configuration file or (if the setting is missing or unspecified) the default converted to a string dictionary.
- `GetSecret` - can be used to get a value from the configuration file's encrypted section (it first checks if there is an unencrypted value in the `appSettings` section).

The library is built as a [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) assembly, so you can use it from either .NET Core or traditional .NET apps. 

### SampleConsoleApp project
The sample project builds a console app that illustrates how to initialize sensitive and non-sensitive application settings.

#### Non-sensitive application settings
It makes sense to encapsulate initialization for the non-sensitive settings in a dedicated static class (`AppConfig` in this sample). As you see, the code calls the `Config` object's `GetValue` method passing the appropriate defaults. `GetValue` will check the specified key in the configuration file's `appSettings` section and if the key does not exist or holds an empty string, it will return the specified default. The method automatically converts the returned setting to the proper data type.

```csharp
using System;
using BasicAppSettings;

namespace Sample
{
    /// <summary>
    /// Sets application settings using the defaults or the values specified in the configuration file.
    /// </summary>
    internal static class AppConfig
    {
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

        internal static string[] Operations =
            Config.GetArray("Operations", "Create|Read|Update|Delete|Assign|Revoke|Enable|Disable", true, "|");

        internal static string[] Objects =
            Config.GetArray("Objects", "User|Group|Role", true, "|");

        internal static Dictionary<string, string> FirstPriority =
            Config.GetDictionary("FirstPriority", "User=1;Group=2;Role=3");

        internal static Dictionary<string, string> SecondPriority =
            Config.GetDictionary("SecondPriority", "User=1;Group=2;Role=3");
    }
}
```

To use a specific setting, the application can simply reference the corresponding static property of the `AppConfig` class:

```csharp
Console.WriteLine(String.Format("{0}= {1}", "Code      ", AppConfig.Code));
Console.WriteLine(String.Format("{0}= {1}", "Index     ", AppConfig.Index));
Console.WriteLine(String.Format("{0}= {1}", "Max       ", AppConfig.Max));
Console.WriteLine(String.Format("{0}= {1}", "Min       ", AppConfig.Min));
Console.WriteLine(String.Format("{0}= {1}", "FirstDate ", AppConfig.FirstDate));
Console.WriteLine(String.Format("{0}= {1}", "LastDate  ", AppConfig.LastDate));
Console.WriteLine(String.Format("{0}= {1}", "Enabled   ", AppConfig.Enabled));
Console.WriteLine(String.Format("{0}= {1}", "Enforce   ", AppConfig.Enforce));
```

This is how you convert an app setting to a string array:

```csharp
string[] operations = AppConfig.Operations;
Console.WriteLine(String.Format("\nOperations:"));
foreach (string operation in operations)
{
    Console.WriteLine(operation);
}
```

And this is how you convert an app setting to a string dictionary:

```csharp
Dictionary<string, string> priority1 = AppConfig.FirstPriority;
Console.WriteLine(String.Format("\nFirst Priority:"));
foreach (string key in priority1.Keys)
{
    Console.WriteLine(String.Format("{0,-5} = {1}", key, priority1[key]));
}
```

Now, whenever you want to override the hard-coded defaults, simply update the `app.config` file's `AppSettings` section like:

```xml
<appSettings>
  <add key="Operations" value="Create|Read|Update|Delete|Assign|Revoke|Enable|Disable|Expire|nexpire"/>
  <add key="Code" value="A"/>
  <add key="Enabled" value="false"/>
  <add key="Max" value="999"/>
  <add key="FirstDate" value="1988-08-08T12:34:00.000"/>
  <add key="Secret1" value="From the 'appSettings' section."/>
  <add key="SecondPriority" value="User=3;Group=2;Role=1"/>
</appSettings>
```

#### Sensitive application settings
For managing sensitive application settings, the code relies on the standard [`aspnet_regiis.exe`](https://blogs.msdn.microsoft.com/gaurav/2013/12/15/encrypting-section-of-config-file-using-aspnet_regiis-exe-the-configuration-for-physical-path-web-config-cannot-be-opened/) tool. The sample project comes with the modified version of the [`Crypt.config.bat`](https://github.com/alekdavis/Crypt.config.bat) script that takes care of the minutiae needed for the key creation and encryption. When you build the project, the post-build step will invoke the `Crypt.config.bat` file to create a key container and then encrypt a sensitive configuration section of the application configuration file in the output (`Debug` or `Release`) folder (notice that once the key container creation step successfully runs, it will keep failing because on repeated attempts it will try to create the container that already exists, so you can ignore this error). 

In the sample, the sensitive settings are stored inside of the `secureAppSettings` section in the configuration file (one setting is stored in the `appSettings` section just to illustrate that you can do it for debugging purposes, but you should never keep sensitive settings unencrypted once the application is deployed). Here what they look like in plain text (once you build the project, the `secureAppSettings` section will be encrypted):

```xml
<configSections>
  <section name="secureAppSettings" type="System.Configuration.AppSettingsSection, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
</configSections>
<configProtectedData defaultProvider="basicConfigurationSampleRsaProvider">
  <providers>
    <remove name="basicConfigurationSampleRsaProvider"/>
    <add name="basicConfigurationSampleRsaProvider" type="System.Configuration.RsaProtectedConfigurationProvider, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" keyContainerName="sampleRsaKey" useMachineContainer="true" />
  </providers>
</configProtectedData>
<appSettings>
  ...
  <add key="Secret1" value="From the 'appSettings' section."/>
</appSettings>
<secureAppSettings>
  <add key="Secret2" value="From the 'secureAppSettings' section."/>
  <add key="Secret3" value="From the 'secureAppSettings' section."/>
</secureAppSettings>
```

Notice that you can export the secure application setting section into an external file (this may make deployment easier) and it will still work.

Once you have the sensitive value defined (and encrypted), the application can retrieve it:

```csharp
string[] names = new string[] { "Secret1", "Secret2", "Secret3" };

foreach (string name in names)
{
    string value = Config.GetSecret<string>(name);
    Console.WriteLine(String.Format("{0} = {1}", name, value));
}
```

## Dependencies
For some [bizarre reasons](https://github.com/dotnet/standard/issues/506), when using the `BasicAppSettings` class library, you need to explicitly reference the [System.Configuration.ConfigurationManager](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) Nuget package from your project.

## Usage
To include the `BasicAppSettings` class library in your project, add the following Nuget packages:

- [BasicAppSettings](https://www.nuget.org/packages/BasicAppSettings)
- [System.Configuration.ConfigurationManager](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) (in case, it is not pulled automatically)

