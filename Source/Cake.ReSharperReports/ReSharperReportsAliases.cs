using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.ReSharperReports.Provider;

namespace Cake.ReSharperReports
{
    /// <summary>
    /// Contains aliases related to ReSharperReports API
    /// </summary>
    [CakeAliasCategory("ReSharperReports")]
    public static class ReSharperReportsAliases
    {
        /// <summary>
        /// Gets a <see cref="ReSharperReportsProvider"/> instance that can be used to transform ReSharper XML Reports into human readable version
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="ReSharperReportsProvider"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.ReSharperReports.Provider")]
        [CakeNamespaceImport("Cake.ReSharperReports.Transform")]
        public static ReSharperReportsProvider ReSharperReports(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new ReSharperReportsProvider(context);
        }
    }
}