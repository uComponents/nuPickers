using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using ClientDependency.Core;
using ClientDependency.Core.CompositeFiles;
using ClientDependency.Core.CompositeFiles.Providers;

namespace nuPickers
{
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
                //the file must have failed to open
                return false;
            }
        }

        public IVirtualFileProvider FileProvider
        {
            get { return new EmbeddedResourceVirtualPathProvider(); }
        }
    }
}
