using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace AzureResources
{
    class Provider : IResourceProvider
    {
        private string _virtualPath;
        private string _className;
        private IDictionary _resourceCache;
        private static object CultureNeutralKey = new object();

        public Provider(string virtualPath, string className)
        {
            _virtualPath = virtualPath;
            _className = className;
        }

        public object GetObject(string resourceKey, CultureInfo culture)
        {
            string cultureName = culture != null ? culture.Name : CultureInfo.CurrentUICulture.Name;
            resourceKey += "." + cultureName;
            object value = GetResourceCache(cultureName)[resourceKey];
            if (value == null)
            {
                Service.AddResource(resourceKey, _virtualPath, _className, cultureName);
                _resourceCache[cultureName] = null;
                value = GetResourceCache(cultureName)[resourceKey];
            }
            return value;
        }

        public IResourceReader ResourceReader
        {
            get
            {
                return new ResourceReader(GetResourceCache(null));
            }
        }

        private IDictionary GetResourceCache(string cultureName)
        {
            object cultureKey;
            if (cultureName != null)
                cultureKey = cultureName;
            else
                cultureKey = CultureNeutralKey;
            if (_resourceCache == null)
                _resourceCache = new ListDictionary();
            IDictionary resourceDict = _resourceCache[cultureKey] as IDictionary;
            if (resourceDict == null)
            {
                resourceDict = Service.GetResources(_virtualPath, _className, cultureName);
                _resourceCache[cultureKey] = resourceDict;
            }
            return resourceDict;
        }
    }
}
