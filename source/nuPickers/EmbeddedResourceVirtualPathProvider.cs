namespace nuPickers
{
    using System;

    using ClientDependency.Core.CompositeFiles;

    /// <summary>
    /// The embedded resource virtual path provider.
    /// </summary>
    public sealed class EmbeddedResourceVirtualPathProvider : IVirtualFileProvider
    {
        /// <summary>
        /// The file exists.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool FileExists(string virtualPath)
        {
            if (!virtualPath.EndsWith(".nu", StringComparison.InvariantCultureIgnoreCase)) return false;

            return EmbeddedResourceHelper.ResourceExists(virtualPath);
        }

        /// <summary>
        /// The get file.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="IVirtualFile"/>.
        /// </returns>
        public IVirtualFile GetFile(string virtualPath)
        {
            if (!FileExists(virtualPath)) return null;

            return new EmbeddedResourceVirtualFile(virtualPath);
        }
    }
}