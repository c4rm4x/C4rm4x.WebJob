#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Framework.Storage
{
    /// <summary>
    /// Service responsible to retrieve/delete documents stored on a third party storage
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Donwloads the content of the document by its url
        /// </summary>
        /// <param name="uri">The document uri</param>
        /// <returns>The instance of the document if this exists</returns>
        Task<byte[]> RetrieveAsync(string uri);

        /// <summary>
        /// Deletes the content of the content by its url
        /// </summary>
        /// <param name="uri">The document uri</param>
        Task DeleteAsync(string uri);
    }
}
