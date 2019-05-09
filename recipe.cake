#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./Source",
                            title: "Cake.ReSharperReports",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.ReSharperReports",
                            appVeyorAccountName: "cakecontrib",
                            shouldRunGitVersion: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/Source/Cake.ReSharperReports.Tests/*.cs", BuildParameters.RootDirectoryPath + "/Source/Cake.ReSharperReports/**/*.AssemblyInfo.cs"  },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();
