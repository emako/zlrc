using CommandLine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZonyLrcTools.Models
{
    public class CommandLineOptions
    {
        public static CommandLineOptions Instance = Parser.Default.ParseArguments<CommandLineOptions>(Environment.GetCommandLineArgs()).Value;

        [Option("", Required = false, HelpText = "Input files to be processed.")]
        public IEnumerable<string> Inputs { get; set; }

        [Option('h', "help", Required = false, HelpText = "Print help.")]
        public bool Help { get; set; }

        [Option('t', "title", Required = false, HelpText = "Set the searching song title and download.")]
        public string Title { get; set; }

        [Option('a', "artist", Required = false, HelpText = "Set the searching song artist and download.")]
        public string Artist { get; set; }

        [Option('o', "output", MetaValue = "FILE", Required = false, HelpText = "Set output filename.")]
        public string Output { get; set; }

        public bool IsEmpty
        {
            get
            {
                PropertyInfo[] pi = GetType().GetProperties();

                foreach (PropertyInfo p in pi)
                {
                    if (p.PropertyType == typeof(string) && p.CanRead)
                    {
                        if (p.GetValue(this) != null)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
