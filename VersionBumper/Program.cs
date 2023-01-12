using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace VersionBumper
{
    internal class Program
    {
        static string hash(string filePath)
        {
            var sha = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(File.ReadAllText(filePath)));

            var res = new StringBuilder();
            for (var x = 0; x < sha.Length; x++)
                res.Append(sha[x].ToString("X2"));
            
            return res.ToString();
        }

        static void Main(string[] arrayArgs)
        {
            List<string> args = arrayArgs.ToList();
            if (args.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid amount of arguments\n\r  Usage: VersionBumper.exe <path to AssemblyInfo.cs>");
                Console.ResetColor();

                Environment.Exit(0);
            } else if (!File.Exists(args[0]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File does not exit");
                Console.ResetColor();

                Environment.Exit(0);
            } else if (!args[0].EndsWith("AssemblyInfo.cs"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect file name");
                Console.ResetColor();

                Environment.Exit(0);
            } else if (!args.Contains("--key"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No --key set, identifier required");
                Console.ResetColor();

                Environment.Exit(0);
            }

            string id = "default";
            for (int x = 0;x < args.Count;x++)
            {
                if (args[x] == "--key")
                {
                    if (args[x + 1].StartsWith("--"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No identifier was paired with --key");
                        Console.ResetColor();

                        Environment.Exit(0);
                    }
                    else
                        id = args[x + 1];
                }
            }

            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\VersionBumper", true);
            if (key == null)
                key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\VersionBumper", true);

            string lastHash = key.GetValue(id)?.ToString();
            string hashed = hash(args[0]);

            if (lastHash == hashed && !args.Contains("--force"))
                Environment.Exit(0);

            key.SetValue(id, hashed);

            string data = File.ReadAllText(args[0]);
            Regex versionRegex = new Regex(@"\[assembly: (AssemblyVersion|AssemblyFileVersion)\(""([0-9.]{0,99})""\)\]");

            foreach (Match match in versionRegex.Matches(data))
            {
                string matchValue = match.Value;
                Version version = Version.Parse(match.Groups[2].ToString());

                if (version.Revision == -1)
                    version = Version.Parse($"{version.Major}.{version.Minor}.{version.Build + 1}");
                else
                    version = Version.Parse($"{version.Major}.{version.Minor}.{version.Build}.{version.Revision + 1}");

                matchValue = matchValue.Replace(match.Groups[2].ToString(), version.ToString());

                data = data.Replace(match.Value, matchValue);
            }

            if (args.Contains("--output"))
                Console.WriteLine(data);

            if (args.Contains("--pause"))
                Console.ReadKey();

            if (!args.Contains("--no-write"))
                File.WriteAllText(args[0], data);
        }
    }
}
