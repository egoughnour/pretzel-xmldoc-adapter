using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;
using NDesk.Options;
using System.IO;

namespace PretzelAdapter
{
    public class Adapter
    {
        public const string BasePathToken = @"<basePathToken>";
        public const string PretzelSiteDirectory = "mdgen";
        public const string PluginDirectory = "_plugins";
        public const string PluginFile = "Plugin.csx";
        public const string BinaryDirectory = "bin";
        public const string PostsDirectory = "_posts";
        public const string OptionsAssemblyRef = "NDesk.Options.dll";

        public static readonly string Slash = new string(Path.DirectorySeparatorChar, 1);

        public static string PostsPath
        {
            get
            {
                return $@"{SitePath}{Slash}{PostsDirectory}";
            }
        }

        public static string AbsoluteBasePath { get; set; }
        public static string SitePath
        {
            get
            {
                return $@"{AbsoluteBasePath}{Slash}{PretzelSiteDirectory}";
            }
        }
        public static string PluginFilePath
        {
            get
            {
                return $@"{SitePath}{Slash}{PluginDirectory}{Slash}{PluginFile}";
            }

        }
        public static string PluginPath
        {
            get
            {
                return $@"{SitePath}{Slash}{PluginDirectory}";
            }

        }

        public static string BinaryFilePath
        {
            get
            {
                return $@"{BinaryPath}{Slash}{OptionsAssemblyRef}";
            }
        }

        public static string BinaryPath
        {
            get
            {
                return $@"{PluginPath}{Slash}{BinaryDirectory}";
            }
        }

        public static string GetPluginSource(Options options)
        {
            return Path.Combine(options.AdapterDirectory, $@"{PluginDirectory}{Slash}{PluginFile}");
        }

        public static string GetOptionsRefSource(Options options)
        {
            return Path.Combine(options.AdapterDirectory, $@"{PluginDirectory}{Slash}{BinaryDirectory}{Slash}{OptionsAssemblyRef}");
        }

        public static void EnsureAllTargetPathsCreated(out bool pathsExist)
        {
            if(!(pathsExist = Directory.Exists(BinaryPath)))
            {
                Directory.CreateDirectory(BinaryPath);
            }
        }

        public static void Main(string[] args)
        {
            var options = Options.GetOptions(args);
            AbsoluteBasePath = Path.GetFullPath(options.BaseDirectory);
            CreateSite();
            var copyUnneeded = default(bool);
            EnsureAllTargetPathsCreated(out copyUnneeded);
            if (!copyUnneeded)
            {
                var optionsRefSource = GetOptionsRefSource(options);
                var pluginSource = GetPluginSource(options);
                var pluginText = File.ReadAllText(pluginSource).Replace(BasePathToken, AbsoluteBasePath);

                File.WriteAllText(PluginFilePath, pluginText);
                File.Copy(optionsRefSource, BinaryFilePath);
            }
            ReInitializePostsDirectory(options);
            Taste(options);
        }

        public static void ReInitializePostsDirectory(Options options)
        {
            if(!Directory.Exists(PostsPath))
            {
                Directory.CreateDirectory(PostsPath);
                CopyInput(options);
            }
            else
            {
                CopyInput(options, true);
            }
        }

        public static void CopyInput(Options options, bool clearPostsPath = false)
        {
            if(clearPostsPath)
            {
                Directory.Delete(PostsPath, true);
                Directory.CreateDirectory(PostsPath);
            }
            //TODO: Copy with globbing and directory input accounted for
        }

        public static void CreateSite()
        {
            if(!Directory.Exists(SitePath))
            {
                if(!Directory.Exists(AbsoluteBasePath))
                {
                    Directory.CreateDirectory(AbsoluteBasePath);
                }
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = "pretzel create mdgen",
                        WorkingDirectory = AbsoluteBasePath
                    }
                };
                process.Start();
                process.WaitForExit();
            }
        }

        public static void Taste(Options options)
        {
            var exitFlag = options.ExitEarly ? " --earlyexit" : string.Empty;
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    Arguments = $@"pretzel taste -t=customxmlmd --project={options.OutputPrefix ?? @""""""}{exitFlag}",
                    WorkingDirectory = SitePath
                }
            };
            process.Start();
            process.WaitForExit(); 
        }
        //TODO make sure all paths are relative to the basepath provided.

    }
}
