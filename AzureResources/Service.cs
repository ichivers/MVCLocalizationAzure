using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureResources
{
    public static class Service
    {
        private static CloudTable table
        {
            get
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("resources");
                table.CreateIfNotExists();
                return table;
            }
        }

        public static IDictionary GetResources(string virtualPath, string className, string cultureName)
        {
            TableQuery<Entity> rangeQuery = null;
            if (!String.IsNullOrEmpty(virtualPath))
                if (string.IsNullOrEmpty(cultureName))
                    rangeQuery = new TableQuery<Entity>().Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, virtualPath),
                            TableOperators.And,
                            TableQuery.GenerateFilterCondition("Culture", QueryComparisons.Equal, "")));
                else
                    rangeQuery = new TableQuery<Entity>().Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, virtualPath),
                            TableOperators.And,
                            TableQuery.GenerateFilterCondition("Culture", QueryComparisons.Equal, cultureName)));
            else if (!String.IsNullOrEmpty(className))
                if (string.IsNullOrEmpty(cultureName))
                    rangeQuery = new TableQuery<Entity>().Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, className),
                            TableOperators.And,
                            TableQuery.GenerateFilterCondition("Culture", QueryComparisons.Equal, "")));
                else
                    rangeQuery = new TableQuery<Entity>().Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, className),
                            TableOperators.And,
                            TableQuery.GenerateFilterCondition("Culture", QueryComparisons.Equal, cultureName)));
            else
                throw new Exception("AzureResourceHelper.GetResources() - virtualPath or className missing from parameters.");
            ListDictionary resources = new ListDictionary();           
            foreach (Entity entity in table.ExecuteQuery(rangeQuery))
            {
                resources.Add(entity.RowKey, entity.Value);
            }
            return resources;
        }

        private static void InsertOrMergeResource(Entity resource)
        {
            TableOperation insertOperation = TableOperation.InsertOrMerge(resource);
            table.Execute(insertOperation);
        }

        public static void AddResource(Entity resource)
        {
            InsertOrMergeResource(resource);
        }

        public static void AddResource(string resourceName, string virtualPath, string className, string cultureName)
        {
            Entity resource = new Entity()
            {
                Culture = cultureName,
                Value = resourceName,
                RowKey = resourceName
            };
            if (!String.IsNullOrEmpty(virtualPath))
                resource.PartitionKey = virtualPath;
            else if (!String.IsNullOrEmpty(className))
                resource.PartitionKey = className;            
            InsertOrMergeResource(resource);
        }

        public static async Task<IList<string>> GetResourceClasses()
        {
            IList<string> resourceClasses = new List<string>();
            try
            {
                var tableQuery = new TableQuery<Entity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Index"));
                TableQuerySegment<Entity> tableResult = await table.ExecuteQuerySegmentedAsync<Entity>(tableQuery, null);
                foreach (Entity entity in tableResult)
                {
                    resourceClasses.Add(entity.RowKey);
                }
            }
            catch (StorageException storageException)
            {
                Debug.Write(storageException.Message);
            }
            return resourceClasses;
        }

        public static async Task<IList<Entity>> GetResources(string className)
        {
            IList<Entity> resources = new List<Entity>();
            try
            {
                var tableQuery = new TableQuery<Entity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, className));
                TableQuerySegment<Entity> tableResult = await table.ExecuteQuerySegmentedAsync<Entity>(tableQuery, null);
                foreach (Entity entity in tableResult)
                {
                    resources.Add(entity);
                }
            }
            catch (StorageException storageException)
            {
                Debug.Write(storageException.Message);
            }
            return resources;
        }
    }
}
