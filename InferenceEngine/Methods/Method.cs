using System.Collections.Generic;

namespace InferenceEngine
{
    abstract class Method
    {
        // Name defines how different are referred.
        public string Name { get; internal set; }

        internal List<SentenceElement> KB;
        internal List<SentenceElement> Query;

        /// <summary>
        /// Informs the inference method of known information
        /// </summary>
        /// <param name="aKB">The knowledge base to interpret</param>
        public abstract void Tell(List<SentenceElement> aKB);
        
        /// <summary>
        /// Asks the inference method a query to assess based on what has been told.
        /// </summary>
        /// <param name="aKB">The query to assess</param>
        /// <returns>String assessment of search method. what is the result of the search</returns>
        public abstract string Ask(List<SentenceElement> aQuery);

    }
}
