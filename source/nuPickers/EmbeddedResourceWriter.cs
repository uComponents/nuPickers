namespace nuPickers
{
    using System;
    using System.IO;
    using System.Web;

    using ClientDependency.Core;
    using ClientDependency.Core.CompositeFiles;
    using ClientDependency.Core.CompositeFiles.Providers;

    /// <summary>
    /// The embedded resource writer.
    /// </summary>
    public sealed class EmbeddedResourceWriter : IVirtualFileWriter
    {
        /// <summary>
        /// The write to stream.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="sw">
        /// The sw.
        /// </param>
        /// <param name="vf">
        /// The vf.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="origUrl">
        /// The orig url.
        /// </param>
        /// <param name="http">
        /// The http.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
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
