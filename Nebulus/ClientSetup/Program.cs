using System;
using WixSharp;

namespace ClientSetup
{
    class Program
    {
        static void Main()
        {
            
            var project = new Project("NebulusClient",
                             new Dir(@"%ProgramFiles%\My Company\My Product",
                                 new File("Program.cs")));

            project.GUID = new Guid("{891E4204-1239-4000-B296-BCFDB7D24001}");
            
            project.SourceBaseDir =  "<input dir path>";
            //project.OutDir = "<output dir path>";

            project.BuildMsi();
        }
    }
}