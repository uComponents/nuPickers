using System;
using System.Web.Hosting;
using ClientDependency.Core.CompositeFiles;

namespace nuPickers
{
    public sealed class EmbeddedResourceVirtualPathProvider : IVirtualFileProvider
    {
        public bool FileExists(string virtualPath)
        {
            if (!virtualPath.EndsWith(".nu", StringComparison.InvariantCultureIgnoreCase)) return false;

            return EmbeddedResourceHelper.ResourceExists(virtualPath);
        }

        public IVirtualFile GetFile(string virtualPath)
        {
            if (!FileExists(virtualPath)) return null;

            return new EmbeddedResourceVirtualFile(virtualPath);
        }
    }
}