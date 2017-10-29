using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NDesk.Options;

namespace PretzelAdapter
{
    public class Adapter
    {
        public const string PluginDirectoryToken = @"<pathToPluginsDir>";

        static void Main(string[] args)
        {
            //TODO make sure all paths are relative to the basepath provided.


            //COMPILE TIME WORKFLOW
            //TODO: Check if mdgen directory.exists
            //TODO: [PROC] pretzel create mdgen (if DNE)
            //TODO: create mdgen/_plugins directory (if DNE)
            //TODO: replace PluginDirectoryToken in Plugin.csx
            //TODO: copy Plugin.csx to mdgen/_pluginsn (if DNE)
            //TODO: copy static md files to mdgen/_posts (if DNE)
            //TODO: clear mdgen/_posts directory.  copy static md files to mdgen/_posts (if Exists)
            //TODO: [PROC] pretzel taste -t=customxmlmd --project=<projectName> --earlyexit <true|false|t|f|0|1>
            
        }
    }
}
