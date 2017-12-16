using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Localization.Api.v1.Models
{
    public class LocalizationResourceClass
    {
        public string Name { get; set; }
    }

    public class Localization
    {
        public string Culture { get; set; }
        public string Area { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}