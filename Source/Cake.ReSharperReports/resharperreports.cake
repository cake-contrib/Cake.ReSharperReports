#r "Cake.ReSharperReports.dll"

try
{
	ReSharperReports.Transform("C:/temp/dupfinder-output.xml", "c:/temp/dupfinder-output.html");

	ReSharperReports.Transform("C:/temp/inspectcode-output.xml", "c:/temp/inspectcode-output.html");

	ReSharperReports.Transform("C:/temp/dupfinder-output.xml", "c:/temp/dupfinder-output-xsl.html", new ReSharperReportsSettings()
	{
		XslFilePath = "c:/temp/dupfinder.xsl"
	});

	ReSharperReports.Transform("C:/temp/inspectcode-output.xml", "c:/temp/inspectcode-output-xsl.html", new ReSharperReportsSettings()
	{
		XslFilePath = "c:/temp/inspectcode.xsl"
	});

	ReSharperReports.Transform("C:/temp/dupfinder-output.xml", "c:/temp/dupfinder-output-xsl-log.html", new ReSharperReportsSettings()
	{
		XslFilePath = "c:/temp/dupfinder.xsl",
		LogFilePath = "c:/temp/resharperreports-dupfinder.log"
	});

	ReSharperReports.Transform("C:/temp/inspectcode-output.xml", "c:/temp/inspectcode-output-xsl-log.html", new ReSharperReportsSettings()
	{
		XslFilePath = "c:/temp/inspectcode.xsl",
		LogFilePath = "c:/temp/resharperreports-inspectcode.log"
	});
}
catch(Exception ex)
{
    Error("{0}", ex);
}

Console.ReadLine();