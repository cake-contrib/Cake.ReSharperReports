///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target          = Argument<string>("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isLocalBuild        = !AppVeyor.IsRunningOnAppVeyor;
var isPullRequest       = AppVeyor.Environment.PullRequest.IsPullRequest;
var isDevelopBranch     = AppVeyor.Environment.Repository.Branch == "develop";
var isTag               = AppVeyor.Environment.Repository.Tag.IsTag;
var solution            = "./Source/Cake.ReSharperReports.sln";
var solutionPath        = Directory("./Source/Cake.ReSharperReports");
var sourcePath          = Directory("./Source");
var binDir              = Directory("./Source/Cake.ReSharperReports/bin") + Directory(configuration);
var objDir              = Directory("./Source/Cake.ReSharperReports/obj") + Directory(configuration);
var buildArtifacts      = Directory("./BuildArtifacts");
var testResultsDir      = buildArtifacts + Directory("test-results");
var version             = "0.3.0";
var semVersion          = "0.3.0";

var assemblyInfo        = new AssemblyInfoSettings {
                                Title                   = "Cake.ReSharperReports",
                                Description             = "Cake ReSharperReports AddIn",
                                Product                 = "Cake.ReSharperReports",
                                Company                 = "gep13",
                                Version                 = version,
                                FileVersion             = version,
                                InformationalVersion    = semVersion,
                                Copyright               = string.Format("Copyright © gep13 {0} - Present", DateTime.Now.Year),
                                CLSCompliant            = true
                            };
var nuGetPackSettings   = new NuGetPackSettings {
                                Id                      = assemblyInfo.Product,
                                Version                 = assemblyInfo.InformationalVersion,
                                Title                   = assemblyInfo.Title,
                                Authors                 = new[] {assemblyInfo.Company},
                                Owners                  = new[] {assemblyInfo.Company},
                                Description             = assemblyInfo.Description,
                                Summary                 = "Cake AddIn that extends Cake with ability to transform ReSharper Reports into human readable format",
                                ProjectUrl              = new Uri("https://github.com/gep13/Cake.ReSharperReports/"),
                                LicenseUrl              = new Uri("https://github.com/gep13/Cake.ReSharperReports/blob/master/LICENSE"),
                                Copyright               = assemblyInfo.Copyright,
                                ReleaseNotes            = new List<string>() { "https://github.com/gep13/Cake.ReSharperReports/releases" },
                                Tags                    = new [] {"Cake", "Script", "Build", "ReSharperReports"},
                                RequireLicenseAcceptance= false,
                                Symbols                 = false,
                                NoPackageAnalysis       = true,
                                Files                   = new [] {
                                                                    new NuSpecContent {Source = "Cake.ReSharperReports.dll"},
                                                                    new NuSpecContent {Source = "Cake.ReSharperReports.pdb"},
                                                                    new NuSpecContent {Source = "Cake.ReSharperReports.xml"}
                                                                 },
                                BasePath                = binDir,
                                OutputDirectory         = buildArtifacts
                            };

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    Information("Cleaning {0}", solutionPath);
    CleanDirectories(binDir);
    CleanDirectories(objDir);

	Information("Cleaning BuildArtifacts");
	CleanDirectories(buildArtifacts);
});

Task("Restore")
    .Does(() =>
{
    // Restore all NuGet packages.
    Information("Restoring {0}...", solution);
    NuGetRestore(solution);
});

Task("SolutionInfo")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var file = "./Source/SolutionInfo.cs";
    CreateAssemblyInfo(file, assemblyInfo);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("SolutionInfo")
    .IsDependentOn("DupFinder")
	.IsDependentOn("InspectCode")
    .Does(() =>
{
    Information("Building {0}", solution);
    MSBuild(solution, settings =>
        settings.SetPlatformTarget(PlatformTarget.MSIL)
            .WithProperty("TreatWarningsAsErrors","true")
            .WithTarget("Build")
            .SetConfiguration(configuration));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./Source/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings {
        OutputDirectory = testResultsDir,
        XmlReportV1 = true,
        NoAppDomain = true
    });
});

Task("DupFinder")
	.IsDependentOn("Create-BuildArtifacts-Directory")
    .Does(() =>
{
    // Run ReSharper's DupFinder
    DupFinder(solution, new DupFinderSettings() {
      ShowStats = true,
      ShowText = true,
      OutputFile = buildArtifacts + File("_ReSharperReports/dupfinder.xml"),
      });
});

Task("InspectCode")
	.IsDependentOn("Create-BuildArtifacts-Directory")
    .Does(() =>
{
    // Run ReSharper's InspectCode
    InspectCode(solution, new InspectCodeSettings() {
      SolutionWideAnalysis = true,
	  Profile = sourcePath + File("Cake.ReSharperReports.sln.DotSettings"),
      OutputFile = buildArtifacts + File("_ReSharperReports/inspectcode.xml"),
      });
});

Task("Create-BuildArtifacts-Directory")
	.Does(() =>
{
    if (!DirectoryExists(buildArtifacts))
    {
        CreateDirectory(buildArtifacts);
    }

    if (!DirectoryExists(testResultsDir))
    {
        CreateDirectory(testResultsDir);
    }
});

Task("Create-NuGet-Package")
    .IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Create-BuildArtifacts-Directory")
    .Does(() =>
{
    NuGetPack(nuGetPackSettings);
});

Task("Publish-Nuget-Package")
    .IsDependentOn("Create-NuGet-Package")
    .WithCriteria(() => !isLocalBuild)
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    // Resolve the API key.
	var apiKey = EnvironmentVariable("MYGET_DEVELOP_API_KEY");
	if(!isDevelopBranch)
	{
		apiKey = EnvironmentVariable("MYGET_MASTER_API_KEY");
	}

	if(isTag) {
		apiKey = EnvironmentVariable("NUGET_API_KEY");
	}

    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet/Nuget API key.");
    }

    var source = EnvironmentVariable("MYGET_DEVELOP_SOURCE");
	if(!isDevelopBranch)
	{
		source = EnvironmentVariable("MYGET_MASTER_SOURCE");
	}

	if(isTag) {
		source = EnvironmentVariable("NUGET_SOURCE");
	}

    if(string.IsNullOrEmpty(source)) {
        throw new InvalidOperationException("Could not resolve MyGet/Nuget source.");
    }

    // Get the path to the package.
    var package = buildArtifacts + File("./Cake.ReSharperReports." + semVersion + ".nupkg");

    // Push the package.
    NuGetPush(package, new NuGetPushSettings {
        Source = source,
        ApiKey = apiKey
    });
});

Task("Default")
    .IsDependentOn("Create-NuGet-Package");

Task("AppVeyor")
    .IsDependentOn("Publish-Nuget-Package");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
