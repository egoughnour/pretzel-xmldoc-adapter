using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;

namespace PretzelAdapter
{
    public class Options
    {
        public static Options GetOptions(string[] args)
        {
            var showHelp = false;
            var options = new Options();
            options.AdapterDirectory = AssemblyDirectory;
            var optionsToParse = new OptionSet()
            {
                { "b|basepath=", "the path where the pretzel site will be generated.", v => options.BaseDirectory = v },
                { "x|exitearly", "whether to continue processing after reading the front matter", v => options.ExitEarly = (v != null)},
                { "p|project=", "the project for which the templates are to be processed", v => options.OutputPrefix = v},
                { "i|input=", "input file(s), path or file glob", v => options.Input = v },
                { "h|help", "display the associated help text", v => showHelp = (null != v) }
            };
            try
            {
                var unmatchedInput = optionsToParse.Parse(args);

            }
            catch (OptionException oe)
            {
                ShowHelp(optionsToParse, oe.Message);
            }
            if (showHelp)
            {
                ShowHelp(optionsToParse);
            }
            return options;

        }
        private Options()
        {
            BaseDirectory = "";
            ExitEarly = false;
            OutputPrefix = "";
            Input = "";
        }

        public string AdapterDirectory { get; set; }
        public string BaseDirectory { get; set; }
        public bool ExitEarly { get; set; }
        public string OutputPrefix { get; set; }
        public string Input { get; set; }
        static void ShowHelp(OptionSet currentOptionSet, string exceptionMessage = null, bool exitFromHelp = true)
        {
            if(null != exceptionMessage)
            {
                Console.WriteLine(exceptionMessage);
            }
            Console.WriteLine("Usage: pretzelAdapter [Options]");
            Console.WriteLine("Sets up a pretzel site and processes the input files.");
            Console.WriteLine("Extracts file processing options from front matter of the input files.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            currentOptionSet.WriteOptionDescriptions(Console.Out);
            if(exitFromHelp)
            {
                Environment.Exit(0);
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
