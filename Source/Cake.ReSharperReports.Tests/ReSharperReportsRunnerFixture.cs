using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.ReSharperReports.Tests
{
    internal sealed class ReSharperReportsRunnerFixture : ToolFixture<ReSharperReportsSettings>
    {
        public ICakeLog Log { get; set; }

        public FilePath InputFilePath { get; set; }

        public FilePath OutputFilePath { get; set; }

        public ReSharperReportsRunnerFixture()
             : base("ReSharperReports.exe")
        {
            Log = Substitute.For<ICakeLog>();
            InputFilePath = "c:/temp/input.xml";
            OutputFilePath = "c:/temp/output.html";
        }

        protected override void RunTool()
        {
            var tool = new ReSharperReportsRunner(FileSystem, Environment, Globber, ProcessRunner);
            tool.Run(InputFilePath, OutputFilePath, Settings);
        }
    }
}