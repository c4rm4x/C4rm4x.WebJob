#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebJob.Framework.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Storage.AzureBlob
{
    /// <summary>
    /// Implementation of IStorage using Azure Blob
    /// </summary>
    public class Storage : IStorage
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cloudStorageAccount">The cloud storage account</param>
        public Storage(CloudStorageAccount cloudStorageAccount)
        {
            cloudStorageAccount.NotNull(nameof(cloudStorageAccount));

            _cloudStorageAccount = cloudStorageAccount;
        }

        /// <summary>
        /// Deletes the content of the content by its url
        /// </summary>
        /// <param name="uri">The document uri</param>
        public Task DeleteAsync(string uri)
        {
            uri.NotNullOrEmpty(nameof(uri));

            var cloudBlockBlob = new CloudBlockBlob(new Uri(uri));

            return cloudBlockBlob.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Donwloads the content of the document by its url
        /// </summary>
        /// <param name="uri">The document uri</param>
        /// <returns>The instance of the document if this exists</returns>
        public async Task<byte[]> RetrieveAsync(string uri)
        {
            uri.NotNullOrEmpty(nameof(uri));

            var cloudBlockBlob = new CloudBlockBlob(new Uri(uri));

            await cloudBlockBlob.FetchAttributesAsync();

            var content = new byte[cloudBlockBlob.Properties.Length];

            await cloudBlockBlob.DownloadToByteArrayAsync(content, 0);

            return content;
        }
    }
}
