using System.Collections.Generic;

namespace InferenceEngine
{
    abstract class Method
    {
        // Name defines how different are referred.
        public string Name { get; internal set; }

        internal List<SentenceElement> KB;
        internal List<SentenceElement> Query;

        public abstract void Tell(List<SentenceElement> aKB);
        public abstract string Ask(List<SentenceElement> aQuery);

    }
}