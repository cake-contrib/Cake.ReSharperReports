# Build Script

In order to make use of the Cake.ReSharperReports Addin, you will need to take advantage of the built-in support for executing both DupFinder and InspectCode.  You can find details of both of these [here](http://cakebuild.net/dsl/resharper).

It is envisioned that the generating of HTML reports will only be required when a violation of the one of the rules in either DupFinder, or InspectCode has occurred.  As a result, an example build script would look something like this:

## DupFinder

```
Task("RunDupFinder")
    .Does(() =>
{
	DupFinder(solution, new DupFinderSettings() {
      ShowStats = true,
      ShowText = true,
      OutputFile = buildArtifacts + "/_ReSharperReports/dupfinder-output.xml",
      });
})
.OnError(exception =>
{
    ReSharperReports.Transform(buildArtifacts + "/_ReSharperReports/dupfinder-output.xml", buildArtifacts + "/_ReSharperReports/dupfinder-output.html");
});
```

**NOTE:** In this example, the options for both ShowStats and ShowText are enabled.  In order for the generated HTML report to show all required information, these settings have to be enabled.

## InspectCode

```
Task("RunInspectCode")
    .Does(() =>
{
    InspectCode(solution, new InspectCodeSettings() {
      SolutionWideAnalysis = true,
	  Profile = sourcePath + "/ReSharperReports.sln.DotSettings",
      OutputFile = buildArtifacts + "/_ReSharperReports/inspectcode-output.xml",
      });
})
.OnError(exception =>
{
    ReSharperReports.Transform(buildArtifacts + "/_ReSharperReports/inspectcode-output.xml", buildArtifacts + "/_ReSharperReports/inspectcode-output.html");
});
```