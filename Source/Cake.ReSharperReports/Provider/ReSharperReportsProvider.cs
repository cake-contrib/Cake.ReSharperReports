using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.ReSharperReports.Transform;

namespace Cake.ReSharperReports.Provider
{
    /// <summary>
    /// Contains functionality related to ReSharperReports API
    /// </summary>
    public sealed class ReSharperReportsProvider
    {
        private readonly ReSharperReportsRunner _reSharperReportsRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReSharperReportsProvider"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        internal ReSharperReportsProvider(ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _reSharperReportsRunner = new ReSharperReportsRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
        }

        /// <summary>
        /// Sync two folders content
        /// </summary>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="outputFilePath">The output file path.</param>
        public void Transform(FilePath inputFilePath, FilePath outputFilePath)
        {
            if (inputFilePath == null)
            {
                throw new ArgumentNullException(nameof(inputFilePath));
            }

            if (outputFilePath == null)
            {
                throw new ArgumentNullException(nameof(outputFilePath));
            }

            Transform(inputFilePath, outputFilePath, new ReSharperReportsSettings());
        }

        /// <summary>
        /// Sync two folders content
        /// </summary>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// /// <param name="settings">The settings.</param>
        public void Transform(FilePath inputFilePath, FilePath outputFilePath, ReSharperReportsSettings settings)
        {
            if (inputFilePath == null)
            {
                throw new ArgumentNullException(nameof(inputFilePath));
            }

            if (outputFilePath == null)
            {
                throw new ArgumentNullException(nameof(outputFilePath));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _reSharperReportsRunner.Transform(inputFilePath, outputFilePath, settings);
        }
    }
}