namespace nuPickers.EmbeddedResource
{
    using ClientDependency.Core.CompositeFiles;
    using System;

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
            if (!virtualPath.EndsWith(EmbeddedResource.FILE_EXTENSION, StringComparison.InvariantCultureIgnoreCase)) return false;

            string resourceName = EmbeddedResourceHelper.GetResourceNameFromPath(virtualPath);

            return EmbeddedResourceHelper.ResourceExists(resourceName);
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