using AssemblyInfoUtil.Interface;
using Common.Environment;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInfoUtil.Implementation
{
    public class CommandLineUtility : ICommandLineUtility
    {
        private readonly string CommandLinePath;
        //private readonly string BuildCommand;

        public CommandLineUtility()
        {
            CommandLinePath = "cmd.exe";
        }
        private Process GetProcess()
        {
            return new Process();
        }
        private ProcessStartInfo GetProcessStart()
        {
            throw new NotImplementedException();
        }

        private ProcessStartInfo GetProcessStart(string projectPath, string commands)
        {
            //"/C nuget pack"
            var startInfo = new ProcessStartInfo(CommandLinePath, commands);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = projectPath;//"C:\\DllSyncTestFramework";

            return startInfo;
        }

        public string RunCommand( string projectPath, string commands)
        {
            var process = GetProcess();
            var startInfo = GetProcessStart(projectPath, commands);

            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "/K nuget pack";
            //startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.WorkingDirectory = projectPath;//"C:\\DllSyncTestFramework";
            process.StartInfo = startInfo;

            process.Start();

            var output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;

        }

        //public string BuildProject(string projectPath)
        //{
        //    var process = GetProcess();
        //    var startInfo = GetProcessStart(projectPath, BuildCommand);
        //    process.StartInfo = startInfo;

        //    process.Start();

        //    var output = process.StandardOutput.ReadToEnd();

        //    process.WaitForExit();

        //    return output;
        //}
    }
}
