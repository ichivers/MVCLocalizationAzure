using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureResources
{
    public class Entity : TableEntity
    {         
        public string Culture { get; set; }
        public string Value { get; set; }
    }    
}
