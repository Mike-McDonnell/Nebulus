using System;
using WixSharp;

namespace ClientSetup
{
    class Program
    {
        static void Main()
        {
            var project =
            new Project("Nebulus System Client",
                new Dir(@"%ProgramFiles%\BAMC\Nebulus",

                    //new Dir("Documentation", new Files(@"\\BUILDSERVER\My Product\Release\Documentation\*.*")), //uncomment if you have a real remote files to install 

                    new Files(@"..\*.*",
                              f => !f.EndsWith(".obj") &&
                                   !f.EndsWith(".pdb")),
                    
                    new ExeFileShortcut("Uninstall NebulusClient", "[System64Folder]msiexec.exe", "/x [ProductCode]")),
                    new RegValue(Microsoft.Win32.RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "NebulusClient", @"[INSTALLDIR]Nebulus\NebulusClient.exe"),
                    new RegValue(Microsoft.Win32.RegistryHive.Users, @".DEFAULT\Control Panel\Desktop", "SCRNSAVE.EXE", @"[INSTALLDIR]Nebulus\PPTScreenSaver.scr"),
                    new RegValue(Microsoft.Win32.RegistryHive.Users, @".DEFAULT\Control Panel\Desktop", "ScreenSaveTimeOut", "60"),
                    new RegValue(Microsoft.Win32.RegistryHive.Users, @".DEFAULT\Control Panel\Desktop", "ScreenSaveActive", "1"));
            project.SourceBaseDir = @"F:\Projects\Nebulus";

            project.GUID = new Guid("815DCA42-45A9-448A-A812-A576C952D0D0");
            //project.ResolveWildCards(ignoreEmptyDirectories: true)
            //.FindFile((f) => f.Name.EndsWith("MyApp.exe"))
            //.First().
            //.Shortcuts = new[] {
            //                    new FileShortcut("MyApp.exe", "INSTALLDIR"),
            //                    new FileShortcut("MyApp.exe", "%Desktop%")
            //               };

            //project.Platform = Platform.x64;

            project.Version = new Version(1, 0);
            project.ControlPanelInfo.Comments = "Nebulus System Client";
            //project.ControlPanelInfo.HelpLink = "https://wixsharp.codeplex.com/support";
            project.ControlPanelInfo.HelpTelephone = "210-916-7582";
            //project.ControlPanelInfo.UrlInfoAbout = "https://wixsharp.codeplex.com/About";
            //project.ControlPanelInfo.UrlUpdateInfo = "https://wixsharp.codeplex.com/update";
            //project.ControlPanelInfo.ProductIcon = "app_icon.ico";
            project.ControlPanelInfo.Contact = "Michael McDonnell";
            project.ControlPanelInfo.Manufacturer = "Michael McDonnell";
            project.ControlPanelInfo.InstallLocation = "[INSTALLDIR]";
            project.ControlPanelInfo.NoModify = true;

            Compiler.PreserveTempFiles = true;
            Compiler.EmitRelativePaths = false;
            Compiler.BuildMsi(project);
        }
    }
}