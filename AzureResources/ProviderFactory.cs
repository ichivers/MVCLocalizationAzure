using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace AzureResources
{
    public sealed class ProviderFactory : ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new Provider(null, classKey);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            virtualPath = Path.GetFileName(virtualPath);
            return new Provider(virtualPath, null);
        }        
    }
}
