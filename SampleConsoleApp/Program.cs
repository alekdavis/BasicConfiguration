using System;
using BasicAppSettings;

namespace Sample
{
    /// <summary>
    /// Illustrates how to use application configuration settings.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("APPLICATION SETTINGS\n");

            Console.WriteLine(String.Format("{0}= {1}", "Operations", AppConfig.Operations));
            Console.WriteLine(String.Format("{0}= {1}", "Objects   ", AppConfig.Objects));
            Console.WriteLine(String.Format("{0}= {1}", "Code      ", AppConfig.Code));
            Console.WriteLine(String.Format("{0}= {1}", "Index     ", AppConfig.Index));
            Console.WriteLine(String.Format("{0}= {1}", "Max       ", AppConfig.Max));
            Console.WriteLine(String.Format("{0}= {1}", "Min       ", AppConfig.Min));
            Console.WriteLine(String.Format("{0}= {1}", "FirstDate ", AppConfig.FirstDate));
            Console.WriteLine(String.Format("{0}= {1}", "LastDate  ", AppConfig.LastDate));
            Console.WriteLine(String.Format("{0}= {1}", "Enabled   ", AppConfig.Enabled));
            Console.WriteLine(String.Format("{0}= {1}", "Enforce   ", AppConfig.Enforce));

            Console.WriteLine("\nAPPLICATION SECRETS\n");

            string[] names = new string[] { "Secret1", "Secret2", "Secret3" };

            foreach (string name in names)
            {
                string value = Config.GetSecret<string>(name);
                Console.WriteLine(String.Format("{0}   = {1}", name, value));
            }

            // OPUTPUT:
            //
            // APPLICATION SETTINGS
            //
            // Operations= Create|Read|Update|Delete|Assign|Revoke|Enable|Disable|Expire|Unexpire
            // Objects   = User|Group|Role
            // Code      = A
            // Index     = X
            // Max       = 999
            // Min       = 5
            // FirstDate = 8/8/1988 12:34:00 PM
            // LastDate  = 7/18/2019 11:38:19 PM
            // Enabled   = False
            // Enforce   = True
            //
            // APPLICATION SECRETS
            //
            // Secret1   = From the 'appSettings' section.
            // Secret2   = From the 'secureAppSettings' section.
            // Secret3   = From the 'secureAppSettings' section.
        }
    }
}
