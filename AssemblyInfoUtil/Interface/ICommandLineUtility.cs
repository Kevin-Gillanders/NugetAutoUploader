using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInfoUtil.Interface
{
    public interface ICommandLineUtility
    {
        public string RunCommand(string projectPath, string command);
        //public string BuildProject(string projectPath);

    }
}
