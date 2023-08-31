// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Common.Environment;
using AssemblyInfoUtil.Interface;
using AssemblyInfoUtil.Implementation;
using AssemblyInfoUtil;



//Loading in appsettings
var configuration = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json");

//Setting up dependancy injection for 
var services = new ServiceCollection();
var config = configuration.Build();


//Loading in data section in appsettings into an equivalent object
services.Configure<ProjectConfig>(options =>
    config.GetSection("ProjectConfig")
    .Bind(options, c => c.BindNonPublicProperties = true));


services.Configure<NugetConfig>(options =>
    config.GetSection("NugetConfig")
    .Bind(options, c => c.BindNonPublicProperties = true));

//Adding the main file to the services to set up the DI chain
services.AddScoped<IAssemblyInfoUtility, AssemblyInfoUtility>();
services.AddScoped<ICommandLineUtility, CommandLineUtility>();
services.AddScoped<DllSyncer>();

//Building service provider and accessing main function to begin program
var serviceProvider = services.BuildServiceProvider();

var dllSyncer = serviceProvider.GetService<DllSyncer>();
//var assembly = serviceProvider.GetService<AssemblyInfoUtility>();


//assembly.UpdateAssembly();

dllSyncer.Run();


//string apiKey = "oy2hcdgrczobmay7knnqtq3jwpb7qu4ejgodw2ioj43oh4";
//string server = "https://api.nuget.org/v3/index.json";
//var process = new System.Diagnostics.Process();
//var startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C nuget pack");

////startInfo.FileName = "cmd.exe";
////startInfo.Arguments = "/K nuget pack";
//startInfo.UseShellExecute = false;
//startInfo.CreateNoWindow = true;
//startInfo.RedirectStandardOutput = true;
//startInfo.RedirectStandardError = true;
//startInfo.WorkingDirectory = "C:\\DllSyncTestFramework";
//process.StartInfo = startInfo;

//process.Start();


//var output = process.StandardOutput.ReadToEnd();
//// write output to console
//Console.WriteLine(output);

//process.WaitForExit();



//var command = $"/C nuget push DllSyncTestFramework.1.1.2.1.nupkg -ApiKey {apiKey} -Source {server}"; 

//process = new System.Diagnostics.Process();
//startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", command);


//startInfo.UseShellExecute = false;
//startInfo.CreateNoWindow = true;
//startInfo.RedirectStandardOutput = true;
//startInfo.RedirectStandardError = true;
//startInfo.WorkingDirectory = "C:\\DllSyncTestFramework";
//process.StartInfo = startInfo;

//process.Start();

//output = process.StandardOutput.ReadToEnd();
//// write output to console
//Console.WriteLine(output);

//process.WaitForExit();

//var cmd = System.Diagnostics.Process.Start("CMD.exe");

//cmd.StartInfo.WorkingDirectory = "C:\\DllSyncTestFramework";


//System.Diagnostics.Process.Start("CMD.exe", "/K dir");
//System.Diagnostics.Process.Start("CMD.exe", "/K nuget pack");

//using (PowerShell ps = PowerShell.Create())
//{
//    // specify the script code to run.
//    ps.AddScript(scriptContents);

//    // specify the parameters to pass into the script.
//    ps.AddParameters(scriptParameters);

//    // execute the script and await the result.
//    var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);

//    // print the resulting pipeline objects to the console.
//    foreach (var item in pipelineObjects)
//    {
//        Console.WriteLine(item.BaseObject.ToString());
//    }
//}

