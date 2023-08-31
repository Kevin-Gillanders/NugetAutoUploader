using AssemblyInfoUtil.Interface;
using Common.Environment;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssemblyInfoUtil.Implementation
{
    public class AssemblyInfoUtility : IAssemblyInfoUtility
    {
        private readonly ProjectConfig projectConfig;

        public AssemblyInfoUtility(IOptions<ProjectConfig> projectConfig)
        {
            this.projectConfig = projectConfig.Value;
        }
        public string UpdateAssembly()
        {
            string newVersionId = "";
            string path = "C:\\DllSyncTestFramework";

            string folder = "Properties";
            string fileName = "AssemblyInfo.cs";


            string assemblyFilePath = Path.Combine(path, folder, fileName);

            var assemblyFile = File.ReadAllText(assemblyFilePath).Split("\r\n");

            Console.Write($"What level do you want to increment [{projectConfig.MinLevel}-{projectConfig.MaxLevel}]? ");

            if (!int.TryParse(Console.ReadLine(), out int level))
                throw new Exception("Level value provided was not valid number");
            if (level < projectConfig.MinLevel)
                throw new Exception($"Level less than {projectConfig.MinLevel}");
            if (level > projectConfig.MaxLevel)
                throw new Exception($"Level was greater than {projectConfig.MaxLevel}");

            //looping over every line of assemblyinfo
            for (int idx = 0; idx < assemblyFile.Length; idx++)
            {
                var line = assemblyFile[idx];
                if (CheckIfLineIsAssemblyVersion(line))
                {
                    Console.WriteLine(line);
                    var updatedAssembly = UpdateAssemblyVersion(line, level);
                    newVersionId = GetVersionId(updatedAssembly);
                    Console.WriteLine(updatedAssembly);
                    assemblyFile[idx] = updatedAssembly;
                }
            }
            File.WriteAllText(assemblyFilePath, string.Join("\r\n", assemblyFile));

            return newVersionId;
        }

        private string GetVersionId(string updatedAssembly)
        {

            string pattern = "\"(.*)\"";
            var idNumbers = Regex.Match(updatedAssembly, pattern).Groups[1].Value;

            return idNumbers;
        }

        private string UpdateAssemblyVersion(string line, int level)
        {
            //[assembly: AssemblyVersion("1.0.0.6")]
            //Grabbing the id numbers and converting them into a list of ints
            string pattern = "\"(.*)\"";
            var idNumbers = Regex.Match(line, pattern).Groups[1].Value.Split(".").Select(int.Parse).ToArray();

            //Updating the given level
            idNumbers[level - 1]++;
            //Set all lower levels to 0
            for (int idx = level; idx < idNumbers.Length; idx++)
                idNumbers[idx] = 0;

            string updatedIDs = "\"" + string.Join(".", idNumbers) + "\"";
            string updatedAssembly = Regex.Replace(line, pattern, updatedIDs);

            return updatedAssembly;

        }

        private bool CheckIfLineIsAssemblyVersion(string line)
        {
            if (line.StartsWith("[assembly: AssemblyVersion"))
                return true;
            else if (line.StartsWith("[assembly: AssemblyFileVersion"))
                return true;

            return false;
        }
    }
}
