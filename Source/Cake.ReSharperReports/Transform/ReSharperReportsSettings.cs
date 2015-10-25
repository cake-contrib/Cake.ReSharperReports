using Cake.Core.IO;

namespace Cake.ReSharperReports.Transform
{
    /// <summary>
    /// Contains settings used by <see cref="ReSharperReportsRunner"/>.
    /// </summary>
    public sealed class ReSharperReportsSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the XSL Transform File path.
        /// </summary>
        /// <value>The XSL Transform File path.</value>
        public FilePath XslFilePath { get; set; }

        /// <summary>
        /// Gets or sets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public FilePath LogFilePath { get; set; }
    }
}