namespace nuPickers.EmbeddedResource
{
    using ClientDependency.Core;
    using ClientDependency.Core.CompositeFiles;
    using ClientDependency.Core.CompositeFiles.Providers;
    using System;
    using System.IO;
    using System.Web;

    /// <summary>
    /// The embedded resource writer.
    /// </summary>
    public sealed class EmbeddedResourceWriter : IVirtualFileWriter
    {
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
            catch (Exception)
            {
                // the file must have failed to open
                return false;
            }
        }

        /// <summary>
        /// Gets the file provider.
        /// </summary>
        public IVirtualFileProvider FileProvider
        {
            get { return new EmbeddedResourceVirtualPathProvider(); }
        }
    }
}
