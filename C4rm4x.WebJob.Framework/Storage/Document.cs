namespace C4rm4x.WebJob.Framework.Storage
{
    /// <summary>
    /// The document information (content and hash)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Gets the document content
        /// </summary>
        public byte[] Content { get; private set; }

        /// <summary>
        /// Gets the document hash
        /// </summary>
        public string Hash { get; private set; }

        private Document()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">The content</param>
        /// <param name="hash">The hash</param>
        public Document(byte[] content, string hash)
        {
            Content = content;
            Hash = hash;
        }
    }
}
