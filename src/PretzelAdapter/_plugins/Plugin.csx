#r "<basePathToken>\mdgen\_plugins\bin\NDesk.Options.dll"
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating;
using Pretzel.Logic.Templating.Context;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NDesk.Options;


[SiteEngineInfo(Engine = "customxmlmd")]
public class Processor : ISiteEngine, IHaveCommandLineArgs
{
    public string OutputFileQualifier { get; set; }
    public bool ExitAfterCustomProcessor { get; set; }
    public string AllowableCustomTags { get; set; }
    public void Initialize()
    {

    }

    public void UpdateOptions(OptionSet options)
    {
        options.Add("earlyexit", "if present, this flag stops processing immediately after the plugins have run", v => ExitAfterCustomProcessor = (v != null));
        options.Add("project=", "the project name with which to prefix the output options file", v => OutputFileQualifier = v);
    }

    public string[] GetArguments(string command)
    {
        return command == "taste" ? new[] { "-earlyexit", "-project" } : new string[0];
    }

    public string OutputFileName
    {
        get
        {
            var fileName = String.IsNullOrWhiteSpace(OutputFileQualifier)
                ? "frontMatterOptions.xml"
                : $@"{OutputFileQualifier}_frontMatterOptions.xml";
            return fileName;
        }
    }

    public bool CanProcess(SiteContext context)
    {
        var readmePage = context.Posts.Where(p => p.File.ToLower().EndsWith("readme.md")).FirstOrDefault();
        if (null == readmePage)
        {
            //if no readme.md then return false
            return false;
        }
        if (readmePage.Bag.ContainsKey("mergexmlcomments"))
        {
            Console.WriteLine("about to check 'mergexmlcomments' as bool");
            if (!(bool)(readmePage.Bag["mergexmlcomments"]))
            {
                Console.WriteLine("as boolean 'mergexmlcomments' is false");
                //if there is a mergexmlcomments value in the front matter
                //but it is false
                return false;
            }
            Console.WriteLine("as boolean 'mergexmlcomments' is true");
            AllowableCustomTags = (string)(readmePage.Bag["allowedcustomtags"]);
            return true;
        }
        else
        {
            Console.WriteLine("Bag doesn't contain 'mergexmlcomments'");
            //no mergexmlcomments value
            return false;
        }
    }

    public void Process(SiteContext context, bool skipFileOnError = false)
    {
        Console.WriteLine($@"About to check CanProcess(context) again");
        if (CanProcess(context))
        {
            var frontMatterOptions = new XElement("options",
                   new XElement("MergeXmlOptions", true),
                   new XElement("CustomTags", AllowableCustomTags));
            frontMatterOptions.Save(OutputFileName);
            if (ExitAfterCustomProcessor)
            {
                Environment.Exit(0);
            }
        }
        else
        {
            Console.WriteLine($@"CanProcess(context) is false this time");
        }
    }
}