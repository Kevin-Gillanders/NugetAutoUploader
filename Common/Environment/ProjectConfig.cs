using Common.Enums;

namespace Common.Environment
{
    public class ProjectConfig
    {
        public string BaseProjectPaths { get; set; }
        //public EnvironmentEnum EnvironmentEnum { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
    }
}