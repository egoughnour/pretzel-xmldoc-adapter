using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PretzelAdapter
{
    public class Adapter
    {
        static void Main(string[] args)
        {
            //COMPILE TIME WORKFLOW
            //TODO: Check if mdgen directory.exists
            //TODO: [PROC] pretzel create mdgen (if DNE)
            //TODO: create mdgen/_plugins directory (if DNE)
            //TODO: copy Plugin.csx, ArgParser.csx to mdgen/_pluginsn (if DNE)
            //TODO: copy static md files to mdgen/_posts (if DNE)
            //TODO: clear mdgen/_posts directory.  copy static md files to mdgen/_posts (if Exists)
            //TODO: [PROC] pretzel taste -t=customxmlmd --optionSetName <projectName> --stopBeforeTemplating <true|false|t|f|0|1>
            //DONE
            //CURRENT TASKS
            //TODO: [CURRENT PROJECT] create option parser (for adapter)
            //[include options for project name, input, output, whether to force exit after pulling frontmatter]
            //TODO: [CURRENT PROJECT] create ArgParser.csx 
            //[set a static string value to indicate current project name and whether or not to exit early]
            //[use the project file name to make the <projectName>_frontMatterOptions.xml file name unique to each project]
            // 
        }
    }
}
