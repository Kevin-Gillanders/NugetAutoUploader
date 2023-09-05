using AssemblyInfoUtil.Implementation;
using AssemblyInfoUtil.Interface;
using Common.Environment;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInfoUtil
{
    public class DllSyncer
    {
        private readonly NugetConfig NugetConf;

        private readonly IAssemblyInfoUtility AssemblyUtil;
        private readonly ICommandLineUtility CommandLine;

        public DllSyncer(IAssemblyInfoUtility assemblyUtil, ICommandLineUtility commandLine, IOptions<NugetConfig> nugetConf)
        {
            NugetConf = nugetConf.Value;
            AssemblyUtil = assemblyUtil;
            CommandLine = commandLine;
        }
        public void Run()
        {
            string projectName = "DllSyncTestFramework";
            string projectPath = "C:\\DllSyncTestFramework";
            var buildCommand = "/C msbuild";
            string packCommand = "/C nuget pack";

            Console.WriteLine("========Update Assembly========");
            var assemblyID = AssemblyUtil.UpdateAssembly();
            Console.WriteLine("========Build project========");

            var output = CommandLine.RunCommand(projectPath, buildCommand);

            Console.WriteLine(output);
            
            Console.WriteLine("========Package project========");

            output = CommandLine.RunCommand(projectPath, packCommand);

            Console.WriteLine(output);
            
            Console.WriteLine("========Push project to NuGet server========");
            string nuspecFile = GetNuspecFileName(projectName, projectPath, assemblyID); 
            var pushCommand = $"/C nuget push {nuspecFile} -ApiKey {NugetConf.ApiKey} -Source {NugetConf.Server}";

            output = CommandLine.RunCommand(projectPath, pushCommand);
            Console.WriteLine(output);
        }

        private string GetNuspecFileName(string projectName, string projectPath, string assemblyID)
        {
            string fileName = $"{projectName}.{assemblyID}.nupkg";
            if (File.Exists(Path.Combine(projectPath, fileName)))
            {
                return fileName;
            }
            else
            {
                var assemblyIDSplit = assemblyID.Split(".").Select(int.Parse).ToList();
                if (assemblyIDSplit.LastOrDefault() == 0)
                {
                    assemblyIDSplit.RemoveAt(assemblyIDSplit.Count - 1);
                    string fileOneLessID = $"{projectName}.{string.Join(".", assemblyIDSplit)}.nupkg";
                    if (File.Exists(Path.Combine(projectPath, fileOneLessID)))
                        return fileOneLessID;
                }
            }
            throw new FileNotFoundException($"File could not be found - {fileName}");
        }
    }
}
