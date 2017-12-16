using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AzureResources
{
    class ResourceReader : IResourceReader
    {
        private IDictionary _resources;

        public ResourceReader(IDictionary resources)
        {
            _resources = resources;
        }

        public void Close()
        {
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        public void Dispose()
        {
        }
    }
}
