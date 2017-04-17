namespace nuPickers.EmbeddedResource
{
    using ClientDependency.Core.CompositeFiles;
    using System.IO;

    /// <summary>
    /// The embedded resource virtual file.
    /// </summary>
    internal class EmbeddedResourceVirtualFile : IVirtualFile
    {
        /// <summary>
        /// The virtual file
        /// </summary>
        private readonly string virtualFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceVirtualFile"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Web.Hosting.VirtualFile"/> class. 
        /// </summary>
        /// <param name="virtualFile">
        /// The virtual path to the resource represented by this instance. 
        /// </param>
        public EmbeddedResourceVirtualFile(string virtualFile)
        {
            this.virtualFile = virtualFile;
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public Stream Open()
        {
            string resourceName = EmbeddedResourceHelper.GetResourceNameFromPath(this.virtualFile);

            return EmbeddedResourceHelper.GetResource(resourceName);
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path
        {
            get { return this.virtualFile; }
        }
    }
}