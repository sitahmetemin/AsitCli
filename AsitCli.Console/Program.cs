using Sharprompt;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using static AsitCli.Console.src.Utility.Definition;

public class Program
{
    private static string _projectName;
    private static string _projectPath;
    private static string _projectType;
    //dotnet tool install -g --add-source ./AsitCli.Console


    static void Main(string[] args)
    {
        try
        {
            _projectName = Prompt.Input<string>("Project name");
            _projectType = Prompt.Select<ProjectType>("ProjectType").ToString();
            _projectPath = $"{Directory.GetCurrentDirectory()}/{_projectName}/src";

            Directory.CreateDirectory($"{_projectName}/src");
            Directory.SetCurrentDirectory(_projectPath);
            
            GenerateSln();
            GenerateApps();
            AddSlnReferences();

            Console.WriteLine("Success");
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    public static void GenerateSln()
    {
        ExecCliCommand($"dotnet new sln -n {_projectName}");
    }

    public static void GenerateApps()
    {
        ExecCliCommand($"dotnet new {_projectType} -n {_projectName}");

        foreach (var item in Enum.GetValues(typeof(ProjectLayers)))
        {
            string projectLayerName = $"{_projectName}.{item}";
            ExecCliCommand($"dotnet new classlib -n {projectLayerName}");
            CreateFoldersAndRemoveClass(projectLayerName);
        }
    }

    public static void CreateFoldersAndRemoveClass(object projectLayerName)
    {
        string projectfolderName = $"{_projectPath}/{projectLayerName}";
        Directory.CreateDirectory($"{projectfolderName}/Abstract/Base");
        Directory.CreateDirectory($"{projectfolderName}/Concrete/Base");
        File.Delete($"{projectfolderName}/Class1.cs");
    }

    public static void AddSlnReferences()
    {
        ExecCliCommand($"dotnet sln add {_projectName}/{_projectName}.csproj");

        foreach (var item in Enum.GetValues(typeof(ProjectLayers)))
        {
            ExecCliCommand($"dotnet sln add {_projectName}.{item}/{_projectName}.{item}.csproj");
        }
    }

    public static void ExecCliCommand(string command)
    {
        ProcessStartInfo pr = new ProcessStartInfo()
        {
            Verb = "runas",
            FileName = "cmd.exe",
            Arguments = "/C " + command,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = false
        };
        var proc = Process.Start(pr);

        proc.WaitForExit();
    }


}