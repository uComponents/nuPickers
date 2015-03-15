namespace nuPickers
{
    using System.IO;

    using ClientDependency.Core.CompositeFiles;

    /// <summary>
    /// The embedded resource virtual file.
    /// </summary>
    internal class EmbeddedResourceVirtualFile : IVirtualFile
    {
        /// <summary>
        /// The _virtual path.
        /// </summary>
        private readonly string _virtualPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceVirtualFile"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Web.Hosting.VirtualFile"/> class. 
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path to the resource represented by this instance. 
        /// </param>
        public EmbeddedResourceVirtualFile(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public Stream Open()
        {
            string resourceName;
            return EmbeddedResourceHelper.GetResource(_virtualPath, out resourceName);
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path
        {
            get { return _virtualPath; }
        }
    }
}