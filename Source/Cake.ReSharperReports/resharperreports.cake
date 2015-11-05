#r "Cake.ReSharperReports.dll"

try
{
	ReSharperReports.Transform("./../../../../Examples/dupfinder-output.xml", "./../../../../BuildArtifacts/dupfinder-output.html");

	ReSharperReports.Transform("./../../../../Examples/inspectcode-output.xml", "./../../../../BuildArtifacts/inspectcode-output.html");

	ReSharperReports.Transform("./../../../../Examples/dupfinder-output.xml", "./../../../../BuildArtifacts/dupfinder-output-xsl.html", new ReSharperReportsSettings()
	{
		XslFilePath = "./../../../../Examples/dupfinder.xsl"
	});

	ReSharperReports.Transform("./../../../../Examples/inspectcode-output.xml", "./../../../../BuildArtifacts/inspectcode-output-xsl.html", new ReSharperReportsSettings()
	{
		XslFilePath = "./../../../../Examples/inspectcode.xsl"
	});

	ReSharperReports.Transform("./../../../../Examples/dupfinder-output.xml", "./../../../../BuildArtifacts/dupfinder-output-xsl-log.html", new ReSharperReportsSettings()
	{
		XslFilePath = "./../../../../Examples/dupfinder.xsl",
		LogFilePath = "./../../../../BuildArtifacts/resharperreports-dupfinder.log"
	});

	ReSharperReports.Transform("./../../../../Examples/inspectcode-output.xml", "./../../../../BuildArtifacts/inspectcode-output-xsl-log.html", new ReSharperReportsSettings()
	{
		XslFilePath = "./../../../../Examples/inspectcode.xsl",
		LogFilePath = "./../../../../BuildArtifacts/resharperreports-inspectcode.log"
	});
}
catch(Exception ex)
{
    Error("{0}", ex);
}

Console.ReadLine();