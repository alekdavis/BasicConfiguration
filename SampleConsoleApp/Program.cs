﻿using System;
using System.Collections.Generic;
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
            Console.WriteLine("APPLICATION SETTINGS (VALUES)\n");

            Console.WriteLine(String.Format("{0}= {1}", "Code      ", AppConfig.Code));
            Console.WriteLine(String.Format("{0}= {1}", "Index     ", AppConfig.Index));
            Console.WriteLine(String.Format("{0}= {1}", "Max       ", AppConfig.Max));
            Console.WriteLine(String.Format("{0}= {1}", "Min       ", AppConfig.Min));
            Console.WriteLine(String.Format("{0}= {1}", "FirstDate ", AppConfig.FirstDate));
            Console.WriteLine(String.Format("{0}= {1}", "LastDate  ", AppConfig.LastDate));
            Console.WriteLine(String.Format("{0}= {1}", "Enabled   ", AppConfig.Enabled));
            Console.WriteLine(String.Format("{0}= {1}", "Enforce   ", AppConfig.Enforce));

            Console.WriteLine("\nAPPLICATION SETTINGS (ARRAYS)");

            string[] operations = AppConfig.Operations;
            Console.WriteLine(String.Format("\nOperations:"));
            foreach (string operation in operations)
            {
                Console.WriteLine(operation);
            }

            string[] objects = AppConfig.Objects;
            Console.WriteLine(String.Format("\nObjects:"));
            foreach (string o in objects)
            {
                Console.WriteLine(o);
            }

            Console.WriteLine("\nAPPLICATION SETTINGS (DICTIONARIES)");

            Dictionary<string, string> priority1 = AppConfig.FirstPriority;
            Console.WriteLine(String.Format("\nFirst Priority:"));
            foreach (string key in priority1.Keys)
            {
                Console.WriteLine(String.Format("{0,-5} = {1}", key, priority1[key]));
            }

            Dictionary<string, string> priority2 = AppConfig.SecondPriority;
            Console.WriteLine(String.Format("\nSecond Priority:"));
            foreach (string key in priority2.Keys)
            {
                Console.WriteLine(String.Format("{0,-5} = {1}", key, priority2[key]));
            }

            Console.WriteLine("\nAPPLICATION SECRETS\n");

            string[] names = new string[] { "Secret1", "Secret2", "Secret3" };

            foreach (string name in names)
            {
                string value = Config.GetSecret<string>(name);
                Console.WriteLine(String.Format("{0}   = {1}", name, value));
            }

            // OUTPUT: 

            //APPLICATION SETTINGS (VALUES)

            //Code      = A
            //Index     = X
            //Max       = 999
            //Min       = 5
            //FirstDate = 8/8/1988 12:34:00 PM
            //LastDate  = 12/11/2019 12:39:32 AM
            //Enabled   = False
            //Enforce   = True

            //APPLICATION SETTINGS (ARRAYS)

            //Operations:
            //Create
            //Read
            //Update
            //Delete
            //Assign
            //Revoke
            //Enable
            //Disable
            //Expire
            //Unexpire

            //Objects:
            //User
            //Group
            //Role

            //APPLICATION SETTINGS (DICTIONARIES)

            //First Priority:
            //User  = 1
            //Group = 2
            //Role  = 3

            //Second Priority:
            //User  = 3
            //Group = 2
            //Role  = 1

            //APPLICATION SECRETS

            //Secret1   = From the 'appSettings' section.
            //Secret2   = From the 'secureAppSettings' section.
            //Secret3   = From the 'secureAppSettings' section.
        }
    }
}
