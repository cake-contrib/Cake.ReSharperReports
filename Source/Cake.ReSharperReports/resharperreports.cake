#r "Cake.ReSharperReports.dll"

try
{
	ReSharperReports.Transform("C:/temp/dupfinder-output.xml", "c:/temp/dupfinder-output.html");
}
catch(Exception ex)
{
    Error("{0}", ex);
}

Console.ReadLine();