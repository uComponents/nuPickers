namespace nuPickers.EmbeddedResource
{
    using ClientDependency.Core;
    using ClientDependency.Core.CompositeFiles;
    using ClientDependency.Core.CompositeFiles.Providers;
    using System;
    using System.IO;
    using System.Web;
    using Umbraco.Core.Logging;

    /// <summary>
    /// The embedded resource writer.
    /// </summary>
    public sealed class EmbeddedResourceVirtualFileWriter : IVirtualFileWriter
    {
        private readonly IProfilingLogger _logger;
        public EmbeddedResourceVirtualFileWriter(  IProfilingLogger profilingLogger)
        {
            _logger = profilingLogger;
        }

        public bool WriteToStream(BaseCompositeFileProcessingProvider provider, StreamWriter sw, IVirtualFile vf, ClientDependencyType type, string origUrl, HttpContextBase http)
        {
            try
            {
                using (var readStream = vf.Open())
                using (var streamReader = new StreamReader(readStream))
                {
                    var output = streamReader.ReadToEnd();
                    DefaultFileWriter.WriteContentToStream(provider, sw, output, type, http, origUrl);
                    return true;
                }
            }
            catch (Exception exception)
            {
                _logger.Info<EmbeddedResourceVirtualFileWriter>(exception.Message);

                return false;
            }
        }

        /// <summary>
        /// Gets the file provider.
        /// </summary>
        public IVirtualFileProvider FileProvider
        {
            get { return new EmbeddedResourceVirtualFileProvider(); }
        }
    }
}